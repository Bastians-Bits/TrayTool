# TrayTool
[![Create Executable](https://github.com/Bastians-Bits/TrayTool/actions/workflows/main.yml/badge.svg)](https://github.com/Bastians-Bits/TrayTool/actions/workflows/main.yml)

This programm provides a possible to create shortcuts within the System Tray (the little icon area in the bottom right).
# GUI
The application consists of one view.
It provides the possibility to add, remove and edit the tree-like structure.
![grafik](https://user-images.githubusercontent.com/51163929/135745402-87a528be-fa14-4fbe-a8f5-5ff3f7578c84.png)
## Folder
Folder are a groupig of items in a tree-like structure (it's like you explorer in windows, do I realy have to explain it?)
![grafik](https://user-images.githubusercontent.com/51163929/135745473-66eabc91-6149-4861-ac9d-dbd494e0bcb6.png)
## Items
Items are shortcuts to programs on your computer.

You can:
* name your item
* Browse or enter the path to the item
* Add CLI arguments to you items

### Icons
The icons are automatically loaded from the linked program.

### Arguments
You can add as many CLI arguments to you program as you wish. They are seperated by key and value and concated with a concatenant. The default concetenant is '=' if none is set.
| Key   | Value | Concetenant | Result    |
| ----- | ----- | ----------- | --------- |
| foo   | bar   |             | foo bar   |
| --foo | bar   |             | --foo bar |
| --foo | bar   | =           | --foo=bar |

![grafik](https://user-images.githubusercontent.com/51163929/135745752-aa8c7594-e12d-42fe-9877-fd3661031955.png)

## Seperater
Provide a possibility to spererate items. They are marked by a line and have no configuration to the right.
![grafik](https://user-images.githubusercontent.com/51163929/135745912-35ffa3d1-ea04-4a2c-93c4-6d93c8cd589c.png)

# Close & Minimize to the System Tray
If you close the applciation it will minimize itself to the System Tray. From there you have to possibility to access you shortcuts or terminate the application for good.
![grafik](https://user-images.githubusercontent.com/51163929/135746186-947f9a86-c669-48e3-93fb-4da34fb26ad6.png)

# Navigation

## Mouse
You can drag and drop items with you mouse. The items is always appended after the items you drop in on. If you drop in on a directory, it is appended to the end.

## WASD
You can move items with the WASD keys. W moves it up, s moves it down. A moves it to the right. If you move it out of a directory it will appended before the directory. D moves it to the right. Works only if the item directly below it is a directory. 

## Arrow Keys
Like WASD, but it just moves the focus.

# Requirements
Since it is dependend on the Windows System Tray and native window libraries it goes without saying that you need Windows. I am using and developing it on Windows 10, I have no experience with other Windows versions.

# How do I get it?
Since I haven't figured out how Github works, you can either look into the actions if there is a drop available or clone the repo and compile it yourself.
