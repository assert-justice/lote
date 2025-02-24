
using System;
using Godot;

public class EntPool{
    Pool<Entity> pool;
    public EntPool(Node parent, Func<Entity> fn){
        pool = new(()=>{
            var e = fn();
            e.Pool = pool;
            return e;
        }){
            InitFn = e => {
                parent.AddChild(e);
                e.Init();
            },
            FreeFn = e => {
                parent.RemoveChild(e);
            }
        };
    }
    public Entity GetNew(){
        return pool.GetNew();
    }
    public void Clear(){
        foreach (var e in pool.GetAlive())
        {
            e.Die();
        }
    }
}