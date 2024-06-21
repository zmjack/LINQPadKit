/// <reference path="./tile-map.ts" />

const output = document.getElementById('output') as HTMLElement;

function log(...str: any[]) {
    let span = document.createElement('span');
    span.innerHTML = `${str.join(' ')}<br/>`;
    output?.appendChild(span);
    output.scrollTop = output.scrollHeight;
}

const map = new TileMap('hexagon', 60, 8, 8);
map.onReady = function (sender, detail) {
    log('ready:', detail.rows, detail.cols);
}

map.onCellMouseDown = function (sender, detail) {
    let color = sender.getColor(detail.x, detail.y);
    if (color != undefined) {
        sender.resetCell(detail.x, detail.y);
    } else {
        sender.setColor(detail.x, detail.y, color);
    }
    log(detail.x, detail.y, color);
}

map.onCellColorChange = function (sender, detail) {
}

map.onImageChange = function (sender, detail) {
    log('image change:', detail.image);
}

map.render('panel');
