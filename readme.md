# unity-platformer
===

Based on https://github.com/SebLague/2DPlatformer-Tutorial evolve
in it's own beast.

## Features


Tiles (Asserts/UnityPlatformer/Scripts/Tiles/)

* Moving platforms
* One way platforms (all four directions)
* Ladders
* Boxes (movable)
* Jumpers
* Ropes
* Item (Pickable / Usable)
* Tracks
* ...


Artificial inteligence (Asserts/UnityPlatformer/Scripts/AI/)

* Patrol
* Projectiles
* Jumpers


Character actions (Asserts/UnityPlatformer/Scripts/Character/Actions/)

* Melee attacks
* Wallstick/WallJump
* Push boxes
* Water (liquid) bouyancy
* Climb/Descent Slopes
* Crounch
* Slip down big slopes
* Use items
* ...


Input (Asserts/UnityPlatformer/Scripts/Input/)
* Keyboard / mouse (unity input)
* Wii pad
* CnControls


## Known issues

[https://github.com/llafuente/unity-platformer/labels/bug](https://github.com/llafuente/unity-platformer/labels/bug)

## TODO

[https://github.com/llafuente/unity-platformer/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement](https://github.com/llafuente/unity-platformer/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement)

## Caveats

[https://github.com/llafuente/unity-platformer/labels/caveat](https://github.com/llafuente/unity-platformer/labels/caveat)

## Wiki

The wiki contains the documentation along with the issues above

* [Home](/llafuente/unity-platformer/wiki)
* [Character setup](/llafuente/unity-platformer/wiki/Character-setup)
* [FAQ](/llafuente/unity-platformer/wiki/FAQ)
* [Usage](/llafuente/unity-platformer/wiki/Usage)

## Hotswapping

`unity-platformer` support (mostly) hotswapping.

There are some limitations, like the character current actions is lost: Example: If character is grabbing will fall.

# License

License is MIT Copyright © 2016 Luis Lafuente Morales <llafuente@noboxout.com>

Except a few files that are shared with Sebastian (https://github.com/SebLague)
