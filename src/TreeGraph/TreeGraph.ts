/// <reference path="../Drawing.ts" />
/// <reference path="../HybridCanvas.ts" />

class TreeNode {
    text: string;
    children: TreeNode[];
    isNullNode?: boolean;

    _area_width?: number;
    _area_height?: number;
    _element?: HTMLDivElement;
}

class TreeGraph extends HybridCanvas {
    public override classNames = {
        graph: 'tree-graph',
        sprites_container: 'tree-graph-nodes',
        sprite: 'tree-graph-node',
        sprite_null: 'tree-graph-null-node',
    };

    private node: TreeNode;

    constructor(private marginlr: number = 20, private levelHeight: number = 80) {
        super();
    }

    override refresh(node: TreeNode = undefined) {
        if (this.element == null) throw "Use render before refresh."

        this.clear();
        node = node ?? this.node;
        if (node) {
            this.node = node;
            this.setup_node_core(this.node, 0, 0);
            this.size = {
                width: this.node._area_width,
                height: this.node._area_height,
            };
            this.draw_node_arrows_core(this.node);
        }
    }

    private offset_x(node: TreeNode, offset: number) {
        node._element.style['left'] = `${node._element.offsetLeft + offset}px`;
        if (node.children != null && node.children.length > 0) {
            for (let child of node.children) {
                this.offset_x(child, offset);
            }
        }
    }

    private setup_node_core(node: TreeNode, level: number, left: number) {
        let div = document.createElement('div');
        div.className = node.isNullNode ? this.classNames.sprite_null : this.classNames.sprite;
        div.innerText = node.text;
        node._element = div;
        node._element.style['top'] = `${level * this.levelHeight}px`;
        this.addSprite(node._element, false);

        node.children = node.children?.map(x => {
            if (x != undefined) return x;
            else {
                let nullNode = new TreeNode();
                nullNode.isNullNode = true;
                nullNode.text = 'null';
                return nullNode;
            };
        });

        if (node.children != undefined && node.children.length > 0) {
            for (let key in node.children) {
                let index = parseInt(key);
                let child = node.children[index];
                let _left = left;
                for (let i = 1; i <= index; i++) {
                    _left += node.children[i - 1]._area_width + this.marginlr;
                }
                this.setup_node_core(child, level + 1, _left);
            }

            node._area_width = Math.max(
                node.children.map(x => x._area_width).reduce((a, b) => a + b + this.marginlr),
                node._element.offsetWidth
            );
            node._area_height = Math.max(...node.children.map(x => x._area_height)) + this.levelHeight;

            let firstChild = node.children[0];
            let lastChild = node.children[node.children.length - 1];

            let firstChild_center = (firstChild._element.offsetLeft + firstChild._element.offsetWidth / 2);
            let lastChild_center = (lastChild._element.offsetLeft + lastChild._element.offsetWidth / 2);
            let offsetLeft = firstChild_center + ((lastChild_center - firstChild_center) - node._element.offsetWidth) / 2;

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

    private draw_node_arrows_core(node: TreeNode) {
        if (node.children != null && node.children.length > 0) {
            for (let child of node.children) {
                this.draw_arrow(node._element, child._element);
                this.draw_node_arrows_core(child);
            }
        }
    }

    private draw_arrow(el_from: HTMLElement, el_to: HTMLElement) {
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

        let arrowBase: Position2D = {
            x: end.x - (end.x - start.x) * percent,
            y: end.y - (end.y - start.y) * percent,
        };

        let sin = (end.x - arrowBase.x) / length;
        let cos = (end.y - arrowBase.y) / length;
        let arrowOffsets: Position2D = {
            x: cos * size,
            y: sin * size,
        };

        let arrowPositions: Position2D[] = [{
            x: arrowBase.x + arrowOffsets.x,
            y: arrowBase.y - arrowOffsets.y
        }, {
            x: arrowBase.x - arrowOffsets.x,
            y: arrowBase.y + arrowOffsets.y
        }];

        this.line(start, end);
        this.polygon([end, ...arrowPositions]);
    }
}
