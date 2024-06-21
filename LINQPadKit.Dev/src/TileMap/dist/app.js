"use strict";
/// <reference path="./tile-map.ts" />
var output = document.getElementById('output');
function log() {
    var str = [];
    for (var _i = 0; _i < arguments.length; _i++) {
        str[_i] = arguments[_i];
    }
    var span = document.createElement('span');
    span.innerHTML = "".concat(str.join(' '), "<br/>");
    output === null || output === void 0 ? void 0 : output.appendChild(span);
    output.scrollTop = output.scrollHeight;
}
var map = new TileMap('hexagon', 60, 8, 8);
map.onReady = function (sender, detail) {
    log('ready:', detail.rows, detail.cols);
};
map.onCellMouseDown = function (sender, detail) {
    var color = sender.getColor(detail.x, detail.y);
    if (color != undefined) {
        sender.resetCell(detail.x, detail.y);
    }
    else {
        sender.setColor(detail.x, detail.y, color);
    }
    log(detail.x, detail.y, color);
};
map.onCellColorChange = function (sender, detail) {
};
map.onImageChange = function (sender, detail) {
    log('image change:', detail.image);
};
map.render('panel');
