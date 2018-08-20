# FloorLayout
WPF program to generate floor layouts (walls/doors) via the FloorLayout template. Requires ShapeTemplateLib

Developed with VS 2015

This project includes Edit2DLib. Edit2DLib is code that I'm writing that is cross platform between .NET and Javascript that does 2D drawing on a surface that takes mouse events and renders lines and polygons. The intent is a library that will let me create drawing tools that can be included in Windows projects (like this one) and also be ported to draw on an HTML canvas. The javascript version of this code is used with the current layout tools on the web site.

I had considered trying to keep a 'pure' cross platform version of Edit2DLib that would be linked to, but decided to simply include the code here and allow it to become a .NET only version -- essentially a branch. If multiple people become involved in this app there will inevitably be changes that are platform specific, and that is fine, since this project is designed to focus on .NET
