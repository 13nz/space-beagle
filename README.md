# CS50 Game Development - Final Project
Lena, 2023


A game made with Unity in which the player controls a beagle in space trying to collect treasure and avoid enemies.

The game is a platformer made of rooms which together form a level. The rooms are procedurally generated using cellular automata and can be adjusted by smoothness and fill percent. Each room has a direction, meaning certain sides of it have openings and others are walls. The level is created when the rooms are generated one by one using a pathfinding algorithm which ensures there is always a path from the player spawn point at the top of the level, to the exit door at the bottom. The player can not traverse upwards but can gain more speed and distance by double-jumping or dashing.

When the player passes through the exit door, the level number increases and the level is randomly generated again from the start, each time with one extra room in width and one in height. The background as well as the tilemap are randomly chosen each time from an array of assets. There is also a decorative tilemap in which collision-free decorations are placed on top of ground tiles at random. The background music is also selected at random, except for the menus, which have a set track.

The player must either dodge or kill enemies walking around the level. The enemies are randomly placed on surfaces depending on the surrounding tiles. The color of each enemy is randomly set when they spawn. The enemies can hurt the player, and the player can kill them by stomping on them or dashing onto them. Upon death, enemies can randomly spawn hearts or collectables. Each enemy defeat awards the player one point. 

The collectables are randomly placed on positions guaranteed to be reachable by the player and the chances of each kind of item spawning depends on its rarity and the points it awards. If the player is at full health, hearts cannot be collected.

There is a pause screen which is activated by pressing ESC, a game over screen, and a main mainu screen, all containing buttons which allow the player to play or quit.

The UI displayes the remaining hearts, the score and the current level.

The player retains the points collected and the amount of hearts when the level changes. The game is over once the player has lost all the hearts and then must start again with 0 score. The player can reset the current level by pressing R.