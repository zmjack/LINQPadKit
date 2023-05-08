/// <reference path="./Drawing.ts" />
var DrawingOptions = /** @class */ (function () {
    function DrawingOptions() {
    }
    return DrawingOptions;
}());
var HybridCanvas = /** @class */ (function () {
    function HybridCanvas() {
        this.element_sprites = [];
    }
    Object.defineProperty(HybridCanvas.prototype, "size", {
        set: function (value) {
            this.element_canvas.width = value.width;
            this.element_canvas.height = value.height;
        },
        enumerable: false,
        configurable: true
    });
    HybridCanvas.prototype.render = function (element) {
        var params = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            params[_i - 1] = arguments[_i];
        }
        this.element = element;
        this.element.className = this.classNames.graph;
        this.element_sprites_container = this.element.getElementsByClassName(this.classNames.sprites_container)[0];
        if (this.element_sprites_container == null) {
            var container = document.createElement('div');
            container.className = this.classNames.sprites_container;
            this.element.appendChild(container);
            this.element_sprites_container = container;
        }
        this.element_canvas = this.element.getElementsByTagName("canvas")[0];
        if (this.element_canvas == null) {
            var canvas = document.createElement('canvas');
            this.element.appendChild(canvas);
            this.element_canvas = canvas;
        }
        this.context2D = this.element_canvas.getContext('2d');
        this.refresh.apply(this, params);
    };
    HybridCanvas.prototype.clear = function () {
        this.clearCanvas();
        this.clearSprites();
    };
    HybridCanvas.prototype.clearCanvas = function () {
        this.context2D.clearRect(0, 0, this.element_canvas.width, this.element_canvas.height);
    };
    HybridCanvas.prototype.addSprite = function (sprite, exsist) {
        this.element_sprites.push(sprite);
        if (!exsist)
            this.element_sprites_container.appendChild(sprite);
    };
    HybridCanvas.prototype.clearSprites = function () {
        this.element_sprites_container.innerHTML = '';
        this.element_sprites = [];
    };
    HybridCanvas.prototype.useOptions = function (options) {
        if (options === null || options === void 0 ? void 0 : options.strokeColor) {
            this.context2D.strokeStyle = options.strokeColor;
        }
        if (options === null || options === void 0 ? void 0 : options.fillColor) {
            this.context2D.fillStyle = options.fillColor;
        }
    };
    HybridCanvas.prototype.polygon = function (positions, options) {
        var first = true;
        if (positions.length > 0) {
            this.context2D.beginPath();
            for (var _i = 0, positions_1 = positions; _i < positions_1.length; _i++) {
                var pos = positions_1[_i];
                if (first) {
                    this.context2D.moveTo(pos.x, pos.y);
                    first = false;
                }
                else
                    this.context2D.lineTo(pos.x, pos.y);
            }
            this.context2D.closePath();
            this.useOptions(options);
            this.context2D.stroke();
            this.context2D.fill();
        }
    };
    HybridCanvas.prototype.line = function (from, to, options) {
        this.context2D.beginPath();
        this.context2D.moveTo(from.x, from.y);
        this.context2D.lineTo(to.x, to.y);
        this.context2D.closePath();
        this.useOptions(options);
        this.context2D.stroke();
    };
    HybridCanvas.prototype.rectangle = function (lt, rb, options) {
        this.polygon([lt, { x: rb.x, y: lt.y }, rb, { x: lt.x, y: rb.y }], options);
    };
    return HybridCanvas;
}());
