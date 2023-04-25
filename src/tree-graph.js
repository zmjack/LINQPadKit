var Coord = /** @class */ (function () {
    function Coord() {
    }
    return Coord;
}());
var TreeNode = /** @class */ (function () {
    function TreeNode() {
    }
    return TreeNode;
}());
var TreeGraph = /** @class */ (function () {
    function TreeGraph(element, marginlr, levelHeight) {
        this.element = element;
        this.marginlr = marginlr;
        this.levelHeight = levelHeight;
        this.nullNode = {
            text: 'null',
            children: undefined,
            isNullNode: true,
        };
        var el_nodes = element.getElementsByClassName(TreeGraph.NodesDivClassName);
        if (el_nodes.length == 0)
            throw "The ".concat(TreeGraph.NodesDivClassName, " div is required.");
        var el_canvas = element.getElementsByTagName('canvas');
        if (el_canvas.length == 0)
            throw "The canvas is required.";
        this.element_nodes = el_nodes[0];
        this.element_canvas = el_canvas[0];
        this.context2D = this.element_canvas.getContext('2d');
    }
    TreeGraph.render = function (element, node, marginlr, margintb) {
        if (marginlr === void 0) { marginlr = 20; }
        if (margintb === void 0) { margintb = 80; }
        element.innerHTML = '';
        element.className = 'tree-graph';
        var el_nodes = document.createElement('div');
        el_nodes.className = TreeGraph.NodesDivClassName;
        element.appendChild(el_nodes);
        var el_canvas = document.createElement('canvas');
        element.appendChild(el_canvas);
        var graph = new TreeGraph(element, marginlr, margintb);
        graph.draw(node);
        return graph;
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
    TreeGraph.prototype.clear = function () {
        this.element_nodes.innerHTML = '';
        this.context2D.clearRect(0, 0, this.element_canvas.width, this.element_canvas.height);
    };
    TreeGraph.prototype.refresh = function () {
        this.draw(this.node);
    };
    TreeGraph.prototype.draw = function (node) {
        this.node = node;
        this.clear();
        this.draw_nodes(node);
        this.element_canvas.width = node._area_width;
        this.element_canvas.height = node._area_height;
        this.draw_arrows(node);
    };
    TreeGraph.prototype.draw_nodes = function (node) {
        this.draw_node(node, 0, 0);
    };
    TreeGraph.prototype.draw_node = function (node, level, left) {
        var _this = this;
        var _a;
        var div = document.createElement('div');
        if (node.isNullNode) {
            div.className = TreeGraph.NullNodeClassName;
            div.innerText = 'null';
        }
        else {
            div.className = TreeGraph.NodeClassName;
            div.innerText = node.text;
        }
        node._element = div;
        this.element_nodes.appendChild(node._element);
        node._element.style['top'] = "".concat(level * this.levelHeight, "px");
        node.children = (_a = node.children) === null || _a === void 0 ? void 0 : _a.map(function (x) {
            if (x != undefined)
                return x;
            else
                return _this.nullNode;
        });
        if (node.children != undefined && node.children.length > 0) {
            for (var key in node.children) {
                var index = parseInt(key);
                var child = node.children[index];
                var _left = left;
                for (var i = 1; i <= index; i++) {
                    _left += node.children[i - 1]._area_width + this.marginlr;
                }
                this.draw_node(child, level + 1, _left);
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
    TreeGraph.prototype.draw_arrows = function (node) {
        if (node.children != null && node.children.length > 0) {
            for (var _i = 0, _a = node.children; _i < _a.length; _i++) {
                var child = _a[_i];
                this.draw_arrow(node._element, child._element);
                this.draw_arrows(child);
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
        this.context2D.beginPath();
        this.context2D.moveTo(start.x, start.y);
        this.context2D.lineTo(end.x, end.y);
        this.context2D.moveTo(end.x, end.y);
        for (var _i = 0, arrowPositions_1 = arrowPositions; _i < arrowPositions_1.length; _i++) {
            var pos = arrowPositions_1[_i];
            this.context2D.lineTo(pos.x, pos.y);
        }
        this.context2D.lineTo(end.x, end.y);
        this.context2D.closePath();
        this.context2D.fill();
        this.context2D.stroke();
    };
    TreeGraph.NodesDivClassName = 'tree-graph-nodes';
    TreeGraph.NodeClassName = 'tree-graph-node';
    TreeGraph.NullNodeClassName = 'tree-graph-null-node';
    return TreeGraph;
}());
