# SMBWFC
***
### credits
- uses linhvu14's SMB-clone (https://github.com/linhdvu14/SMB-clone)
- uses selfsame's unity-wave-function-collapse (https://selfsame.itch.io/unitywfc)

***
### controls
- move: left/right
- crouch: down
- jump: z
- dash/fire: x
***

### explanation

there are three scenes to open in Unity, in Assets/, all are different experiments of using of WFC for SMB level design.

#### World W-FC

this use of WFC samples all of the classic World 1-1 and outputs a single overlay. the outputs are mostly unplayable.

<img src = "https://raw.githubusercontent.com/sweet-JP/SMBWFC/master/Screenshots/wfc0.png" width="694">
<img src = "https://raw.githubusercontent.com/sweet-JP/SMBWFC/master/Screenshots/wfc1.png" width="696">

#### World W-FC2

this use of WFC composes two samples - all of the classic World 1-1 to overlay scenery, enemies, and objects, and a second, smaller sample simply for the ground, which outputs to a two tile high overlay to ensure the level is at least somewhat playable.

<img src = "https://raw.githubusercontent.com/sweet-JP/SMBWFC/master/Screenshots/wfc20.png" width="796">
<img src = "https://raw.githubusercontent.com/sweet-JP/SMBWFC/master/Screenshots/wfc21.png" width="698">
<img src = "https://raw.githubusercontent.com/sweet-JP/SMBWFC/master/Screenshots/wfc22.png" width="692">

#### World W-FC3
this use of WFC composes three samples - a ground canvas, as in W-FC2, as well as a scenery canvas and a object/enemy canvas. it outputs to three separate and overlapping overlays. levels are highly playable and more dense than in W-FC2, because the training data for W-FC2 objects and enemies is the same size as the output.

<img src = "https://raw.githubusercontent.com/sweet-JP/SMBWFC/master/Screenshots/wfc30.png" width="834">
<img src = "https://raw.githubusercontent.com/sweet-JP/SMBWFC/master/Screenshots/wfc31.png" width="690">
<img src = "https://raw.githubusercontent.com/sweet-JP/SMBWFC/master/Screenshots/wfc32.png" width="690">

***
### future work
- i'd like a level generator that generates as mario moves - generating in front of you
- i'd to just arrange an entire game of WFC levels
- i'd like to make an infinite level generator, with some kind of high score challenge
***