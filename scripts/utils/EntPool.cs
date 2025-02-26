
using System;
using System.Collections.Generic;
using Godot;

public class EntPool{
    public Pool<Entity> pool;
    public EntPool(Node parent, Func<Entity> fn){
        pool = new(()=>{
            var e = fn();
            e.Pool = this;
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
    public void Free(Entity e){
        pool.Free(e);
    }
    public IEnumerable<Entity> GetAlive(){
        return pool.GetAlive();
    }

    public void Clear(){
        foreach (var e in pool.GetAlive())
        {
            e.Die();
        }
    }
}