Thanks for downloading and playing this game

First of all, I wouldn't be able to make this game if it wasn't for Bardent. https://www.youtube.com/@Bardent
I followed multiple tutorial videos from him as I am still new to unity and C#.

I also utilized other various sources for coming up with some of the game logic.

Features:

Player:
Player Movement Script:
The player has basic movment with the inputs of "WASD" and the Space Bar for Jumping
Shift can be pressed to activate a dash movement where the player is pushed forward in the direction they face
During a dash, the player will not recieve damage

PlayerCombatScript:
The player comes with basic combat functionality.
A 3-hit combo that can "reset" to the first attack animation if the player takes  too long to string the next attack.
Each attack in the 3-hit combo will do different amounts of damage, increasing up till the third hit.
The player is also equipped with the Special Slow-Motion ability
Holding down right-click will put the player in a slow-motion state
During this state, you have about 2-3 seconds before it ends.
If the player inputs the directional inputs of "Left-Down-Right" or "Right-Down-Left" (Keyboard: "ASD" or "DSA"), the player will unleash a special animation attack which deals much more damage.
This mechanic is inspired by street fighter and other various fighting games.

The player's ability module is easily modifiable. You can add your own attack animation and string inputs for a new attack.

PlayerAfterImage Pool/Sprite, Player Stats:
Can be mostly ignored, the after image scripts simply create the function for the player to look ghost-like when dashing
Player stats is used to manage health points of the player and its GUI

Enemies:

Chainsaw Capybara:
Simply moves back and forth utilizing a wall check to turn the capybara when reaching the end of a path. 
The ground check also helps turn around the capybara when reaching a ledge.
The chainsaw will have a rectangle hitbox and will proc when a player collides with it, dealing damage to the player.

Skribble:
A ranged enemy that simply floats and spawns homing projectiles at static intervals.
The homing projectiles will collide with terrain as well.
The projectiles can be dodged with dashing and killing the enemy will stop the projectiles from spawning.

Boss (Jerri): 
Scripted to chase after the player.
When the boss detects the player within a "long range" or "short range", the boss will randomly choose 1 of two attacks based on it's range.
For example, there are two close range attacks, a multi-hit melee combo at close range, and a backup shot that creates distance between the boss and player as the boss attacks.
For the long range, the boss will either stand and rain a volley of 3 arrows towards the player or the boss will jump up and shoot a triple shot downwards in mid-air.
Combined together, the boss makes for a challenging fight.

