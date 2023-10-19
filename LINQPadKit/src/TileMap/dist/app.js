"use strict";
/// <reference path="./tile-map.ts" />
var map = new TileMap('hexagon', 36, 16, 16);
map.onCellMouseDown = function (args) {
    console.log(args.x, args.y);
    map.setColor(args.x, args.y, 'green');
};
map.render('panel');
