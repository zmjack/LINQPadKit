var tailMap_rectangle = new TileMap('rectangle', 50, 8, 7);
tailMap_rectangle.render('rectangle', { bridge: 'rectangle_bridge' });
document.getElementById('rectangle_bridge').addEventListener('cell:mousedown', function (args) {
    var detail = args.detail;
    var x = detail.x;
    var y = detail.y;
    var color = document.getElementById('color').value;
    if (tailMap_rectangle.getColor(x, y) == color) {
        tailMap_rectangle.resetColor(x, y);
    }
    else
        tailMap_rectangle.setColor(x, y, color);
});
var $console = {
    element: document.getElementById('console'),
    log: function (s) {
        var p = document.createElement('p');
        p.innerText = s;
        this.element.appendChild(p);
    },
    text: function (s) {
        var p = document.createElement('p');
        p.innerText = s;
        this.element.innerHTML = null;
        this.element.appendChild(p);
    }
};
var tailMap_hexagon = new TileMap('hexagon', 50, 8, 7);
tailMap_hexagon.render('hexagon', { bridge: 'hexagon_bridge' });
document.getElementById('hexagon_bridge').addEventListener('cell:mousedown', function (args) {
    var detail = args.detail;
    var x = detail.x;
    var y = detail.y;
    var color = document.getElementById('color').value;
    $console.log("x: ".concat(x, ", y: ").concat(y));
    if (tailMap_hexagon.getColor(x, y) == color) {
        tailMap_hexagon.resetColor(x, y);
    }
    else
        tailMap_hexagon.setColor(x, y, color);
});
