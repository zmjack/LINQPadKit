/// <reference path="../Drawing.ts" />
/// <reference path="../HybridCanvas.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __spreadArray = (this && this.__spreadArray) || function (to, from, pack) {
    if (pack || arguments.length === 2) for (var i = 0, l = from.length, ar; i < l; i++) {
        if (ar || !(i in from)) {
            if (!ar) ar = Array.prototype.slice.call(from, 0, i);
            ar[i] = from[i];
        }
    }
    return to.concat(ar || Array.prototype.slice.call(from));
};
var TileMap = /** @class */ (function (_super) {
    __extends(TileMap, _super);
    function TileMap(tileSize, tileWidth, tileHeight) {
        var _this = _super.call(this) || this;
        _this.tileSize = tileSize;
        _this.tileWidth = tileWidth;
        _this.tileHeight = tileHeight;
        _this.classNames = {
            graph: 'tile-map',
            sprites_container: 'tile-map-sprites',
            sprite: 'tile-map-sprite',
        };
        return _this;
    }
    TileMap.prototype.render = function (element) {
        var params = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            params[_i - 1] = arguments[_i];
        }
        _super.prototype.render.apply(this, __spreadArray([element], params, false));
        var sprites = this.element_sprites_container.getElementsByClassName(this.classNames.sprite);
        for (var i = 0; i < sprites.length; i++) {
            var sprite = sprites[i];
            this.addSprite(sprite, true);
        }
        if (this.element_sprites.length != this.tileHeight * this.tileWidth)
            throw "The length(".concat(this.element_sprites.length, ") of sprites must be ").concat(this.tileHeight * this.tileWidth, ".");
    };
    TileMap.prototype.refresh = function () {
        if (this.element == null)
            throw "Use render before refresh.";
        this.clearCanvas();
        this.size = {
            width: this.tileSize * this.tileWidth,
            height: this.tileSize * this.tileHeight,
        };
        this.drawBackground();
    };
    TileMap.prototype.drawBackground = function () {
        for (var y = 0; y < this.tileHeight; y++) {
            for (var x = 0; x < this.tileWidth; x++) {
                this.rectangle({
                    x: x * this.tileSize,
                    y: y * this.tileSize
                }, {
                    x: (x + 1) * this.tileSize,
                    y: (y + 1) * this.tileSize
                }, {
                    fillColor: (y + x) % 2 == 0 ? '#808080' : '#C0C0C0',
                });
            }
        }
    };
    TileMap.prototype.useMap = function (tileHtmlMap) {
        this.tileHtmlMap = tileHtmlMap;
    };
    TileMap.prototype.loadMap = function (map) {
        if (this.element_sprites.length == 0) {
            for (var y = 0; y < this.tileHeight; y++) {
                for (var x = 0; x < this.tileWidth; x++) {
                    this.addSprite(this.createSprite(), false);
                }
            }
        }
        if (this.element_sprites.length != this.tileHeight * this.tileWidth)
            throw "The length(".concat(this.element_sprites.length, ") of sprites must be ").concat(this.tileHeight * this.tileWidth, ".");
        for (var y = 0; y < this.tileHeight; y++) {
            for (var x = 0; x < this.tileWidth; x++) {
                var div = this.element_sprites[y * this.tileHeight + x];
                var value = map[y] ? map[y][x] : undefined;
                div.innerHTML = value ? this.tileHtmlMap[value] : '';
                div.style['top'] = "".concat(y * this.tileSize, "px");
                div.style['left'] = "".concat(x * this.tileSize, "px");
                div.style['width'] = "".concat(this.tileSize, "px");
                div.style['height'] = "".concat(this.tileSize, "px");
            }
        }
    };
    TileMap.prototype.createSprite = function () {
        var div = document.createElement('div');
        div.className = this.classNames.sprite;
        return div;
    };
    return TileMap;
}(HybridCanvas));
