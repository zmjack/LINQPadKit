"use strict";
/// <reference path="./fabric-impl.d.ts" />
var TileMap = /** @class */ (function () {
    function TileMap(shape, cellWidth, rows, cols) {
        this.shape = shape;
        this.cellWidth = cellWidth;
        this.rows = rows;
        this.cols = cols;
        this.cells = [];
        this.onCellMouseDown = null;
        this.onCellColorChange = null;
        this.onImageChange = null;
        this.onReady = null;
        this.cellHeight = this.cellWidth / 2 * Math.sqrt(3) / 2 * 3;
    }
    TileMap.prototype.render = function (id, net_bridge) {
        this.net_bridge = net_bridge;
        var element = document.getElementById(id);
        if (element == null)
            throw "No element found.";
        element.innerHTML = '';
        var canvas = document.createElement('canvas');
        if (canvas == null)
            throw "No canvas found.";
        if (this.shape == 'rectangle') {
            canvas.width = this.cellWidth * this.cols;
            canvas.height = this.cellWidth * this.rows;
        }
        else if (this.shape == 'hexagon') {
            canvas.width = this.cellWidth * this.cols + this.cellWidth / 2;
            canvas.height = (this.cellWidth / 2 * Math.sqrt(3) / 2) * (this.rows * 2 + 1);
        }
        element.appendChild(canvas);
        this.canvas = canvas;
        this.fabricCanvas = new fabric.Canvas(canvas);
        this.fabricCanvas.selection = false;
        this.init(this.shape);
        this.ready({ rows: this.rows, cols: this.cols });
    };
    TileMap.prototype.init = function (shape) {
        var _this = this;
        if (this.fabricCanvas == null)
            throw "No fabric canvas found.";
        this.cells = [];
        if (shape == 'rectangle') {
            for (var y = 0; y < this.rows; y++) {
                var row = [];
                var _loop_1 = function (x) {
                    var rect = new fabric.Rect({
                        left: x * this_1.cellWidth,
                        top: y * this_1.cellWidth,
                        fill: this_1.getOriginColor(y, x),
                        width: this_1.cellWidth,
                        height: this_1.cellWidth,
                        selectable: false,
                        hoverCursor: 'default'
                    });
                    var cell = { element: rect, y: y, x: x };
                    rect.on('mousedown', function (e) {
                        _this.mouseDown({ x: cell.x, y: cell.y });
                    });
                    this_1.fabricCanvas.add(rect);
                    row.push(cell);
                };
                var this_1 = this;
                for (var x = 0; x < this.cols; x++) {
                    _loop_1(x);
                }
                this.cells.push(row);
            }
        }
        else if (shape == 'hexagon') {
            for (var y = 0; y < this.rows; y++) {
                var row = [];
                var _loop_2 = function (x) {
                    var height = this_2.cellWidth / 2 * Math.sqrt(3) / 2;
                    var left = x * this_2.cellWidth + (y % 2 == 0 ? 0 : this_2.cellWidth / 2);
                    var top_1 = y * this_2.cellWidth / 2 * Math.sqrt(3);
                    var points = [
                        [left + Math.floor(this_2.cellWidth / 2), top_1],
                        [left + this_2.cellWidth, top_1 + height],
                        [left + this_2.cellWidth, top_1 + height * 2],
                        [left + Math.floor(this_2.cellWidth / 2), top_1 + height * 3],
                        [left, top_1 + height * 2],
                        [left, top_1 + height],
                    ];
                    var path = new fabric.Path("M ".concat(points.map(function (p) { return "".concat(p[0], " ").concat(p[1]); }).join(' L '), " z"));
                    path.fill = this_2.getOriginColor(y, x);
                    path.selectable = false;
                    path.hoverCursor = 'default';
                    var cell = {
                        element: path,
                        y: y,
                        x: x,
                        color: path.fill,
                    };
                    path.on('mousedown', function (e) {
                        _this.mouseDown({ x: cell.x, y: cell.y });
                    });
                    this_2.fabricCanvas.add(path);
                    row.push(cell);
                };
                var this_2 = this;
                for (var x = 0; x < this.cols; x++) {
                    _loop_2(x);
                }
                this.cells.push(row);
            }
        }
    };
    ;
    TileMap.prototype.setImage = function (x, y, image) {
        var _this = this;
        var img = new Image();
        img.src = image;
        img.onload = function () {
            var _a;
            if (_this.fabricCanvas == null)
                throw "No fabric canvas found.";
            var pattern = new fabric.Pattern({
                source: img,
                repeat: 'no-repeat',
                offsetX: (_this.cellWidth / 2) - (img.width / 2),
                offsetY: (_this.cellHeight / 2) - (img.height / 2)
            });
            var cell = _this.cells[y][x];
            cell.color = undefined;
            cell.image = image;
            cell.element.set('fill', pattern);
            _this.fabricCanvas.renderAll();
            (_a = _this.imageChange) === null || _a === void 0 ? void 0 : _a.call(_this, { x: x, y: y, image: image });
        };
    };
    TileMap.prototype.getImage = function (x, y) {
        var cell = this.cells[y][x];
        return cell.image;
    };
    TileMap.prototype.setColor = function (x, y, color) {
        var _a;
        if (this.fabricCanvas == null)
            throw "No fabric canvas found.";
        var cell = this.cells[y][x];
        cell.image = undefined;
        cell.color = color;
        cell.element.set('fill', color);
        this.fabricCanvas.renderAll();
        (_a = this.colorChange) === null || _a === void 0 ? void 0 : _a.call(this, { x: x, y: y, color: color });
    };
    TileMap.prototype.getColor = function (x, y) {
        var cell = this.cells[y][x];
        return cell.color;
    };
    TileMap.prototype.resetCell = function (x, y) {
        var color = this.getOriginColor(x, y);
        this.setColor(x, y, color);
    };
    TileMap.prototype.getOriginColor = function (x, y) {
        return '#C0C0C0';
    };
    TileMap.prototype.callBridge = function (name, detail) {
        if (this.net_bridge) {
            var element = document.getElementById(this.net_bridge);
            if (element == null)
                throw "No element found.";
            var _event = new CustomEvent(name, { detail: detail });
            element.dispatchEvent(_event);
        }
    };
    TileMap.prototype.mouseDown = function (detail) {
        var _a;
        (_a = this.onCellMouseDown) === null || _a === void 0 ? void 0 : _a.call(this, this, detail);
        this.callBridge('cell:mouseDown', detail);
    };
    TileMap.prototype.colorChange = function (detail) {
        var _a;
        (_a = this.onCellColorChange) === null || _a === void 0 ? void 0 : _a.call(this, this, detail);
        this.callBridge('cell:colorChange', detail);
    };
    TileMap.prototype.imageChange = function (detail) {
        var _a;
        (_a = this.onImageChange) === null || _a === void 0 ? void 0 : _a.call(this, this, detail);
        this.callBridge('cell:imageChange', detail);
    };
    TileMap.prototype.ready = function (detail) {
        var _a;
        (_a = this.onReady) === null || _a === void 0 ? void 0 : _a.call(this, this, detail);
        this.callBridge('ready', detail);
    };
    return TileMap;
}());
