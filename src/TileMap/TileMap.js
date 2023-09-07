/// <reference path="../fabric-impl.d.ts" />
var TileMap = /** @class */ (function () {
    function TileMap(shape, size, width, height) {
        this.shape = shape;
        this.size = size;
        this.width = width;
        this.height = height;
    }
    TileMap.prototype.render = function (id, params) {
        if (params === null || params === void 0 ? void 0 : params.bridge) {
            var el_bridge = document.getElementById(params.bridge);
            if (el_bridge == null)
                throw "Invalid bridge id.";
            this.bridge = params.bridge;
        }
        var element = document.getElementById(id);
        element.innerHTML = '';
        var canvas = document.createElement('canvas');
        if (this.shape == 'rectangle') {
            canvas.width = this.size * this.width;
            canvas.height = this.size * this.height;
        }
        else if (this.shape == 'hexagon') {
            canvas.width = this.size * this.width + this.size / 2;
            canvas.height = (this.size / 2 * Math.sqrt(3) / 2) * (this.height * 2 + 1);
        }
        element.appendChild(canvas);
        this.canvas = canvas;
        this.fabricCanvas = new fabric.Canvas(canvas);
        this.fabricCanvas.selection = false;
        this.init(this.shape);
    };
    TileMap.prototype.setImage = function (x, y, color) {
        var cell = this.cells[y][x];
        cell.element.set('fill', color);
        this.fabricCanvas.renderAll();
    };
    TileMap.prototype.setColor = function (x, y, color) {
        var cell = this.cells[y][x];
        cell.element.set('fill', color);
        this.fabricCanvas.renderAll();
    };
    TileMap.prototype.getColor = function (x, y) {
        var cell = this.cells[y][x];
        return cell.element.get('fill');
    };
    TileMap.prototype.resetColor = function (x, y) {
        var cell = this.cells[y][x];
        cell.element.set('fill', this.getOriginColor(x, y));
        this.fabricCanvas.renderAll();
    };
    TileMap.prototype.getOriginColor = function (x, y) {
        return '#C0C0C0';
    };
    TileMap.prototype.init = function (shape) {
        var _this = this;
        this.cells = [];
        if (shape == 'rectangle') {
            for (var y = 0; y < this.height; y++) {
                var row = [];
                var _loop_1 = function (x) {
                    var rect = new fabric.Rect({
                        left: x * this_1.size,
                        top: y * this_1.size,
                        fill: this_1.getOriginColor(y, x),
                        width: this_1.size,
                        height: this_1.size,
                        selectable: false,
                        hoverCursor: 'default'
                    });
                    var cell = { element: rect, y: y, x: x };
                    rect.on('mousedown', function (e) {
                        _this.mousedown(_this.bridge, 'cell:mousedown', { x: cell.x, y: cell.y });
                    });
                    this_1.fabricCanvas.add(rect);
                    row.push(cell);
                };
                var this_1 = this;
                for (var x = 0; x < this.width; x++) {
                    _loop_1(x);
                }
                this.cells.push(row);
            }
        }
        else if (shape == 'hexagon') {
            for (var y = 0; y < this.height; y++) {
                var row = [];
                var _loop_2 = function (x) {
                    var height_d2 = this_2.size / 2 * Math.sqrt(3) / 2;
                    var left = x * this_2.size + (y % 2 == 0 ? 0 : this_2.size / 2);
                    var top_1 = y * this_2.size / 2 * Math.sqrt(3);
                    var points = [
                        [left + Math.floor(this_2.size / 2), top_1],
                        [left + this_2.size, top_1 + height_d2],
                        [left + this_2.size, top_1 + height_d2 * 2],
                        [left + Math.floor(this_2.size / 2), top_1 + height_d2 * 3],
                        [left, top_1 + height_d2 * 2],
                        [left, top_1 + height_d2],
                    ];
                    var path = new fabric.Path("M ".concat(points.map(function (p) { return "".concat(p[0], " ").concat(p[1]); }).join(' L '), " z"));
                    path.fill = this_2.getOriginColor(y, x);
                    path.selectable = false;
                    path.hoverCursor = 'default';
                    var cell = { element: path, y: y, x: x };
                    path.on('mousedown', function (e) {
                        _this.mousedown(_this.bridge, 'cell:mousedown', { x: cell.x, y: cell.y });
                    });
                    this_2.fabricCanvas.add(path);
                    row.push(cell);
                };
                var this_2 = this;
                for (var x = 0; x < this.width; x++) {
                    _loop_2(x);
                }
                this.cells.push(row);
            }
        }
    };
    TileMap.prototype.mousedown = function (id, event, detail) {
        var _event = new CustomEvent(event, { detail: detail });
        var element = document.getElementById(id);
        element.dispatchEvent(_event);
    };
    return TileMap;
}());
