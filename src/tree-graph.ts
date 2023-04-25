class Coord {
    x: number;
    y: number;
}

class TreeNode {
    text: string;
    children: TreeNode[];

    _area_width: number;
    _area_height: number;
    _element: HTMLDivElement;
}

class TreeGraph {
    private static readonly NodesDivClassName = 'tree-graph-nodes';
    private static readonly NodeClassName = 'tree-graph-node';
    private element_nodes: HTMLElement;
    private element_canvas: HTMLCanvasElement;
    private context2D: CanvasRenderingContext2D;
    private node: TreeNode;

    private constructor(private element: HTMLElement, private marginlr: number, private levelHeight: number) {
        let el_nodes = element.getElementsByClassName(TreeGraph.NodesDivClassName);
        if (el_nodes.length == 0) throw `The ${TreeGraph.NodesDivClassName} div is required.`;
        let el_canvas = element.getElementsByTagName('canvas');
        if (el_canvas.length == 0) throw "The canvas is required.";

        this.element_nodes = el_nodes[0] as HTMLElement;
        this.element_canvas = el_canvas[0] as HTMLCanvasElement;
        this.context2D = this.element_canvas.getContext('2d');
    }

    static render(element: HTMLElement, node: TreeNode, marginlr = 20, margintb = 80): TreeGraph {
        element.innerHTML = '';
        element.className = 'tree-graph';

        let el_nodes = document.createElement('div');
        el_nodes.className = TreeGraph.NodesDivClassName;
        element.appendChild(el_nodes);

        let el_canvas = document.createElement('canvas');
        element.appendChild(el_canvas);

        let graph = new TreeGraph(element, marginlr, margintb);
        graph.draw(node);
        return graph;
    }

    private offset_x(node: TreeNode, offset: number) {
        node._element.style['left'] = `${node._element.offsetLeft + offset}px`;
        if (node.children != null && node.children.length > 0) {
            for (let child of node.children) {
                this.offset_x(child, offset);
            }
        }
    }

    clear() {
        this.element_nodes.innerHTML = '';
        this.context2D.clearRect(0, 0, this.element_canvas.width, this.element_canvas.height);
    }

    refresh() {
        this.draw(this.node);
    }

    draw(node: TreeNode) {
        this.node = node;
        this.clear();
        this.draw_nodes(node);
        this.element_canvas.width = node._area_width;
        this.element_canvas.height = node._area_height;
        this.draw_arrows(node);
    }

    private draw_nodes(node: TreeNode) {
        this.draw_node(node, 0, 0);
    }

    private draw_node(node: TreeNode, level: number, left: number) {
        let div = document.createElement('div');
        div.className = TreeGraph.NodeClassName;
        div.innerText = node.text;
        node._element = div;
        this.element_nodes.appendChild(node._element);

        node._element.style['top'] = `${level * this.levelHeight}px`;
        node.children = node.children?.filter(x => x != null);

        if (node.children != null && node.children.length > 0) {
            for (let key in node.children) {
                let index = parseInt(key);
                let child = node.children[index];
                let _left = left;
                for (let i = 1; i <= index; i++) {
                    _left += node.children[i - 1]._area_width + this.marginlr;
                }
                this.draw_node(child, level + 1, _left);
            }

            node._area_width = Math.max(
                node.children.map(x => x._area_width).reduce((a, b) => a + b + this.marginlr),
                node._element.offsetWidth
            );
            node._area_height = Math.max(...node.children.map(x => x._area_height)) + this.levelHeight;

            let firstChild = node.children[0];
            let lastChild = node.children[node.children.length - 1];
            let chilren_width = lastChild._element.offsetLeft + lastChild._element.offsetWidth - firstChild._element.offsetLeft;
            let offsetLeft = firstChild._element.offsetLeft + (chilren_width - node._element.offsetWidth) / 2;
            node._element.style['left'] = `${offsetLeft}px`;

            if (offsetLeft < left) {
                this.offset_x(node, left - offsetLeft);
            }
        } else {
            node._area_width = node._element.offsetWidth;
            node._area_height = node._element.offsetHeight;
            node._element.style['left'] = `${left}px`;
        }
    }

    draw_arrows(node: TreeNode) {
        if (node.children != null && node.children.length > 0) {
            for (let child of node.children) {
                this.draw_arrow(node._element, child._element);
                this.draw_arrows(child);
            }
        }
    }

    draw_arrow(el_from: HTMLElement, el_to: HTMLElement) {
        let start = {
            x: el_from.offsetLeft + el_from.offsetWidth / 2,
            y: el_from.offsetTop + el_from.offsetHeight - 1
        };
        let end = {
            x: el_to.offsetLeft + el_to.offsetWidth / 2,
            y: el_to.offsetTop - 1
        };

        let length = 18;
        let size = 3;

        let lineLength = Math.sqrt(Math.pow(end.y - start.y, 2) + Math.pow(end.x - start.x, 2));
        let percent = length / lineLength;

        let arrowBase = {
            x: end.x - (end.x - start.x) * percent,
            y: end.y - (end.y - start.y) * percent,
        };

        let sin = (end.x - arrowBase.x) / length;
        let cos = (end.y - arrowBase.y) / length;
        let arrowOffsets = {
            x: cos * size,
            y: sin * size,
        };

        let arrowPositions = [{
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
        for (let pos of arrowPositions) {
            this.context2D.lineTo(pos.x, pos.y);
        }
        this.context2D.lineTo(end.x, end.y);
        this.context2D.closePath();
        this.context2D.fill();
        this.context2D.stroke();
    }
}
