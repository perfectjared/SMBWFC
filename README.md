#SMBWFC
***
###credits
- uses linhvu14's SMB-clone (https://github.com/linhdvu14/SMB-clone)
- uses selfsame's unity-wave-function-collapse (https://selfsame.itch.io/unitywfc)

***
###controls
- move: left/right
- crouch: down
- jump: z
- dash/fire: x
***

###explanation

there are three scenes to open in Unity, in Assets/, all are different experiments of using of WFC for SMB level design.

#### World W-FC

this use of WFC samples all of the classic World 1-1 and outputs a single overlay. the outputs are mostly unplayable.

<img src = "" width="694">
<img src = "" width="696">

#### World W-FC2

this use of WFC composes two samples - all of the classic World 1-1 to overlay scenery, enemies, and objects, and a second, smaller sample simply for the ground, which outputs to a two tile high overlay to ensure the level is at least somewhat playable.

<img src = "" width="796">
<img src = "" width="698">
<img src = "" width="692">

#### World W-FC3
this use of WFC composes three samples - a ground canvas, as in W-FC2, as well as a scenery canvas and a object/enemy canvas. it outputs to three separate and overlapping overlays. levels are highly playable and more dense than in W-FC2, because the training data for W-FC2 objects and enemies is the same size as the output.

<img src = "" width="834">
<img src = "" width="690">
<img src = "" width="690">