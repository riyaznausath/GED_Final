# GED Final

## How to Play:
- WASD to move
- Mouse to look around
- SPACE to jump
- LMB to attack
- ESC to quit game

## Build:
You can fine the exe file in the published release.

## Scripts:
- EnemyController.cs : used to control enemy's actions such as patrolling and attacking
- GameControls.cs : used for defining input actions for player such as moving
- GameplayManager.cs : used to hide the mouse cursor in play mode
- PlayerController.cs : used to control player's actions such as move, attack, rotate, and jump
- PlayerInputController.cs : global access to Game input actions using singleton pattern
- PlayerSword.cs : used to manage player's sword collision with enemies

## Notes:
A player, two enemies, and platforms are placed in the scene. Enemies are able to move around and attack using AI. 
Some animations were added to all the characters.
One singleton pattern is used in PlayerInputController.cs. No other design patterns or elements such as collectables and win/lose condition were implemented.<br />
3D Model Resource: https://quaternius.com/packs/cyberpunkgamekit.html
