Final Project Concept - The Caveman / Barbarian

The caveman needs to collect metal ores for his tribe, he needs to find one special ore in order to become the chief. He needs to fight off wild animals (and other people) and collect ore in order to find
the one special one to bring back to become the chief. The caveman needs to eat food (like fruit) in order to not starve. The game ends when the caveman finds the special ore. 

Log 1 (7/4/2026) - 

Setting up the map and all the basic stuff. 

Future stuff - making enemy npcs, collection feature, changing player character model, hunger mechanic, combat mechanic.

Log 2 (7/5/2026) - 

Finished setting up all the basic stuff. UI mostly complete.

Future stuff - Change out / add music, change the character model, implement a hunger system, adjust the time cycle, refine the terrain, implement a weather system, implement a combat system (enemy npcs and AI), 
implement menu system, and implement the win condition (the special Ore).

Log 3 (7/5/2026) - 

BUG UPDATE --- I replaced the "new terrain" asset that was missing with "new terrain 1". I effectivbely replaced the terrain from scratch.

Log 4 (7/5/2026) - 

Unfortunately, I was unable to get the new player character model working so I opted to keep the "tutorial assets" that the professor used. For the next checkpoint I'll need to figure out how to replace the
playable character model (to the caveman). Other than that I managed to implement the win condition where if the player finds the special ore, then it will trigger the end game. For now, it just 
recognizes it in the debug console that it is the unique ore. Eventually I will change it so that it restarts the game, and eventually a "You Win" screen to notify completion. Also I'll eventually need to make a 
special ore spawner, so that it will spawn the ore in a random location on the map (as well as a nearest ore marker on the compass to make the game more intuitive). I started working on a script for a
weather cycle system (rain), but I'll need to find additional assets to complete it. I also turned down the music volume because it was a little loud. I will also need to create bondaries/invisible wall around the map so that the playable character does not fall off the map.

I understand that the game looks a lot like the one that was made in the lectures. My hope was to refine things, and make changes (like switching out player models), but technical difficulties like losing the terrain
asset (this ended up happening twice now) prevented me from making this customization changes. Despite this, I have a clear vision on what I need to do, and I would even like to make a desert and or snow version
of this world (a snow and desert version of the game as seperate scenes). 

BUG UPDATE --- For whatever reason whenever I do some pushes into github, I'll sometimes lose my terrain asset, and the only way I have been able to fix the terrain is by straight up redoing the terrain.
Now the terrain is asset is "New Terrain 2". I'm unsure whether or not this happens because I did not save the scene before pushing or if it's something else. But if anyone is aware of this problem,
I would like to know what to do (or not do) in order to avoid having this issue again.
