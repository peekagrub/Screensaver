# Screensaver

Adds a screensaver that moves over the screen

# How to use

Put the directory containing the screensaver in the `Mods/Screensaver/Screensavers` directory. \
Then launch the game and in `Options > Mods > Screensaver` use the `Select Screensaver` or `Screensaver List` options to select the screensaver. \
In `Screensaver Options` are extra options like how many times the screensaver is displayed, how big they are or how fast they move.

Example of the directory structure:
```
Mods
├── Screensaver
│   ├── Screensaver.dll
│   ├── Screensaver.pdb
│   └── Screensavers
│       ├── Default
│       │   └── screensaver.png
│       └── NewScreensaver
│           └── screensaver.png
│
└── Other Mods
```

# How to create a screensaver

Supported image files are PNG, JPG and JPEG.

## Single image screensaver

For a single image screensaver all you have to do is put it in a folder and name the folder what you want the screensaver to be called. \

## Animated screensaver

For an animated screensaver you need to put all the images inside the same folder and create a `screensaver.json` file. 
In this file you will have to explicitly say the screensaver is animated, how long the frames take or the frames per second, and which image files to use. \
You have to use either `"FrameTime"` or `"FPS"`, if both are defined `"FrameTime"` will be used. The image files have to be listed in the order that they are used in the animation.

Example of a `screensaver.json`:
```js
{
    "IsAnimated": true,
    "FrameTime": 0.07,
    "FileNames": [
        "frame1.png",
        "frame2.png",
        "frame3.png",
        "frame4.png",
        "frame5.png",
        "frame6.png",
        "frame7.png",
        "frame8.png",
        "frame9.png",
        "frame10.png"
    ]
}

```

# Credits

TheMathGeek_314 for the art. \
peekagrub for the code.

# EUPL
                      Copyright (c) 2025 peekagrub
                      Licensed under the EUPL-1.2
https://joinup.ec.europa.eu/collection/eupl/eupl-text-eupl-12
