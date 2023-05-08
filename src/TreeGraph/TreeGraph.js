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
var TreeNode = /** @class */ (function () {
    function TreeNode() {
    }
    return TreeNode;
}());
var TreeGraph = /** @class */ (function (_super) {
    __extends(TreeGraph, _super);
    function TreeGraph(marginlr, levelHeight) {
        if (marginlr === void 0) { marginlr = 20; }
        if (levelHeight === void 0) { levelHeight = 80; }
        var _this = _super.call(this) || this;
        _this.marginlr = marginlr;
        _this.levelHeight = levelHeight;
        _this.classNames = {
            graph: 'tree-graph',
            sprites_container: 'tree-graph-nodes',
            sprite: 'tree-graph-node',
            sprite_null: 'tree-graph-null-node',
        };
        return _this;
    }
    TreeGraph.prototype.refresh = function (node) {
        if (node === void 0) { node = undefined; }
        if (this.element == null)
            throw "Use render before refresh.";
        this.clear();
        node = node !== null && node !== void 0 ? node : this.node;
        if (node) {
            this.node = node;
            this.setup_node_core(this.node, 0, 0);
            this.size = {
                width: this.node._area_width,
                height: this.node._area_height,
            };
            this.draw_node_arrows_core(this.node);
        }
    };
    TreeGraph.prototype.offset_x = function (node, offset) {
        node._element.style['left'] = "".concat(node._element.offsetLeft + offset, "px");
        if (node.children != null && node.children.length > 0) {
            for (var _i = 0, _a = node.children; _i < _a.length; _i++) {
                var child = _a[_i];
                this.offset_x(child, offset);
            }
        }
    };
    TreeGraph.prototype.setup_node_core = function (node, level, left) {
        var _this = this;
        var _a;
        var div = document.createElement('div');
        div.className = node.isNullNode ? this.classNames.sprite_null : this.classNames.sprite;
        div.innerText = node.text;
        node._element = div;
        node._element.style['top'] = "".concat(level * this.levelHeight, "px");
        this.addSprite(node._element, false);
        node.children = (_a = node.children) === null || _a === void 0 ? void 0 : _a.map(function (x) {
            if (x != undefined)
                return x;
            else {
                var nullNode = new TreeNode();
                nullNode.isNullNode = true;
                nullNode.text = 'null';
                return nullNode;
            }
            ;
        });
        if (node.children != undefined && node.children.length > 0) {
            for (var key in node.children) {
                var index = parseInt(key);
                var child = node.children[index];
                var _left = left;
                for (var i = 1; i <= index; i++) {
                    _left += node.children[i - 1]._area_width + this.marginlr;
                }
                this.setup_node_core(child, level + 1, _left);
            }
            node._area_width = Math.max(node.children.map(function (x) { return x._area_width; }).reduce(function (a, b) { return a + b + _this.marginlr; }), node._element.offsetWidth);
            node._area_height = Math.max.apply(Math, node.children.map(function (x) { return x._area_height; })) + this.levelHeight;
            var firstChild = node.children[0];
            var lastChild = node.children[node.children.length - 1];
            var firstChild_center = (firstChild._element.offsetLeft + firstChild._element.offsetWidth / 2);
            var lastChild_center = (lastChild._element.offsetLeft + lastChild._element.offsetWidth / 2);
            var offsetLeft = firstChild_center + ((lastChild_center - firstChild_center) - node._element.offsetWidth) / 2;
            node._element.style['left'] = "".concat(offsetLeft, "px");
            if (offsetLeft < left) {
                this.offset_x(node, left - offsetLeft);
            }
        }
        else {
            node._area_width = node._element.offsetWidth;
            node._area_height = node._element.offsetHeight;
            node._element.style['left'] = "".concat(left, "px");
        }
    };
    TreeGraph.prototype.draw_node_arrows_core = function (node) {
        if (node.children != null && node.children.length > 0) {
            for (var _i = 0, _a = node.children; _i < _a.length; _i++) {
                var child = _a[_i];
                this.draw_arrow(node._element, child._element);
                this.draw_node_arrows_core(child);
            }
        }
    };
    TreeGraph.prototype.draw_arrow = function (el_from, el_to) {
        var start = {
            x: el_from.offsetLeft + el_from.offsetWidth / 2,
            y: el_from.offsetTop + el_from.offsetHeight - 1
        };
        var end = {
            x: el_to.offsetLeft + el_to.offsetWidth / 2,
            y: el_to.offsetTop - 1
        };
        var length = 18;
        var size = 3;
        var lineLength = Math.sqrt(Math.pow(end.y - start.y, 2) + Math.pow(end.x - start.x, 2));
        var percent = length / lineLength;
        var arrowBase = {
            x: end.x - (end.x - start.x) * percent,
            y: end.y - (end.y - start.y) * percent,
        };
        var sin = (end.x - arrowBase.x) / length;
        var cos = (end.y - arrowBase.y) / length;
        var arrowOffsets = {
            x: cos * size,
            y: sin * size,
        };
        var arrowPositions = [{
                x: arrowBase.x + arrowOffsets.x,
                y: arrowBase.y - arrowOffsets.y
            }, {
                x: arrowBase.x - arrowOffsets.x,
                y: arrowBase.y + arrowOffsets.y
            }];
        this.line(start, end);
        this.polygon(__spreadArray([end], arrowPositions, true));
    };
    return TreeGraph;
}(HybridCanvas));
