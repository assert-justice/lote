EXTERNAL print(str)
EXTERNAL set_menu(str)
EXTERNAL quit()
EXTERNAL launch()

-> intro

=== intro ===

In the bleak future of... next Thursday evil corporations control everything. This shouldn't be news to you.
What might be news is that you are a courier, transporting necessities like medicine and indy games to the deprived masses.
 ~ set_menu("MainMenu")
-> begin

=== begin ===
You rev your mag cycle and get ready to do some mutual aid... with a vengeance!
 ~ launch()
 -> denouement

=== denouement ===

Once upon a time...
 ~ temp health = 10
 ~ print(health)
You had {health} health!

 * There were two choices. [] Like so
 * There were four lines of content.

- They lived happily ever after.
 ~ quit()
    -> END
