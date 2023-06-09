# Level design  

Each level is procedurally generated from a set of predefined rooms.  
There are 7 levels in total, each with a distinct theme and set of monsters.  
From the 1st to the 6th level, the difficulty increases gradually, with the 7th level being the most challenging.  

Each on on the first 6 levels must contain one and only one of the following rooms:  
- **Entrance**: The starting room of the level.  
- **Treasure**: A room containing an ancient item.  
- **Shop**: A room where player can exchange rare souls for items.  
- **Absence of reality**: A room that can contain everything, from a treasure to a monster. Not accessible trough the regular path.  
- **Guardian**: A room containing a powerful guardian.  
- **Exit**: The path between levels.

The 7th level doesn't not contain treasure or shop rooms, it is a succession of hard rooms that contain powerfull enemies.  
The final boss of the abyss is in a room that is always at the end of the level.  

## Rooms

Each room is a 2D grid of tiles rotated by 45 degrees.  
The size of the room can vary but will always be larger than the camera, so the player need to move around to see the whole room.  
They can contain traps, monsters, enigms, a combination of those or nothing at all.  

## Treasure rooms

They contain an item chosen randomly from the ancient item pool.
There is a chance to find a lore item in the treasure room in addition to the ancient item.

## Shops  

The shop is guarded by a merchant selling items in exchange of rare souls.  
Items proposed are chosen randomly from the ancient item pool as well as in treasure rooms.  
There is a chance to find a lore item in the shop room, this one is not for sale and can be picked up for free.  

## Absence of reality rooms  

Absence of realities are hidden rooms, the player need to search for the entrance.  
They can be very rewarding, fully empty or even very dangerous places.  
If an item is in the room, it is chosen randomly from the ancient artifact pool.
There is a chance to find a lore item in the absence of reality room.  

## Guardian rooms

Guardian rooms are the most dangerous rooms of the level.  
They contain a powerful guardian that the player need to defeat to progress.  
There is always a lore item in the guardian room that tells the player more about the guardian.  
The guardian is chosen randomly from the 3 guardians of the level.  
After beating a guardiant, the player can choose a reward from the ancient item pool.

## Exit rooms  

Just after the guardian room, there is the exit room leading to the next level.  
It has a chance to contain a lore item, that tells the player more about the beaten level story.  

This path is different on the 6th level, it contain an alternative exit that lead the player to a hard challenge room.  
The player can choose a reward from the ancient artifact pool after beating the challenge.


