/// <reference path="./fabric-impl.d.ts" />
type Shape = 'rectangle' | 'hexagon';

interface Cell {
    element: fabric.Object,
    x: number,
    y: number,
}

class TileMap {
    net_bridge?: string;
    canvas?: HTMLCanvasElement;
    fabricCanvas?: fabric.Canvas;
    cells: Cell[][] = [];

    onCellMouseDown: ((args: { x: number, y: number }) => void) | null = null;

    constructor(public shape: Shape, public size: number, public width: number, public height: number) {
    }

    render(id: string, net_bridge?: string) {
        this.net_bridge = net_bridge;

        let element = document.getElementById(id);
        if (element == null) throw "No element found.";

        element.innerHTML = '';
        let canvas = document.createElement('canvas');
        if (canvas == null) throw "No canvas found.";

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
    }

    init(shape) {
        if (this.fabricCanvas == null) throw "No fabric canvas found."

        this.cells = [];
        if (shape == 'rectangle') {
            for (let y = 0; y < this.height; y++) {
                let row: Cell[] = [];
                for (let x = 0; x < this.width; x++) {
                    let rect = new fabric.Rect({
                        left: x * this.size,
                        top: y * this.size,
                        fill: this.getOriginColor(y, x),
                        width: this.size,
                        height: this.size,
                        selectable: false,
                        hoverCursor: 'default'
                    });
                    let cell: Cell = { element: rect, y: y, x: x };
                    rect.on('mousedown', e => {
                        this.mousedown(this.net_bridge, 'cell:mousedown', {
                            x: cell.x,
                            y: cell.y
                        });
                    });
                    this.fabricCanvas.add(rect);
                    row.push(cell);
                }
                this.cells.push(row);
            }
        }
        else if (shape == 'hexagon') {
            for (let y = 0; y < this.height; y++) {
                let row: Cell[] = [];
                for (let x = 0; x < this.width; x++) {
                    let height = this.size / 2 * Math.sqrt(3) / 2;
                    let left = x * this.size + (y % 2 == 0 ? 0 : this.size / 2);
                    let top = y * this.size / 2 * Math.sqrt(3);
                    let points = [
                        [left + Math.floor(this.size / 2), top],
                        [left + this.size, top + height],
                        [left + this.size, top + height * 2],
                        [left + Math.floor(this.size / 2), top + height * 3],
                        [left, top + height * 2],
                        [left, top + height],
                    ];

                    let path = new fabric.Path(`M ${points.map(p => `${p[0]} ${p[1]}`).join(' L ')} z`);
                    path.fill = this.getOriginColor(y, x);
                    path.selectable = false;
                    path.hoverCursor = 'default';

                    let cell: Cell = { element: path, y: y, x: x };
                    console.log(cell);
                    path.on('mousedown', e => {
                        const args = { x: cell.x, y: cell.y };
                        if (this.net_bridge) {
                            this.mousedown(this.net_bridge, 'cell:mousedown', args);
                        }
                        this.onCellMouseDown?.(args);
                    });
                    this.fabricCanvas.add(path);
                    row.push(cell);
                }
                this.cells.push(row);
            }
        }
    };

    setImage(x, y, color) {
        let cell = this.cells[y][x];
        cell.element.set('fill', color);

        if (this.fabricCanvas == null) throw "No fabric canvas found."
        this.fabricCanvas.renderAll();
    }

    setColor(x, y, color) {
        let cell = this.cells[y][x];
        cell.element.set('fill', color);

        if (this.fabricCanvas == null) throw "No fabric canvas found."
        this.fabricCanvas.renderAll();
    }

    getColor(x, y) {
        let cell = this.cells[y][x];
        return cell.element.get('fill');
    }

    resetColor(x, y) {
        let cell = this.cells[y][x];
        cell.element.set('fill', this.getOriginColor(x, y));

        if (this.fabricCanvas == null) throw "No fabric canvas found."
        this.fabricCanvas.renderAll();
    }

    getOriginColor(x, y) {
        return '#C0C0C0';
    }

    mousedown(id, event, detail) {
        let _event = new CustomEvent(event, { detail: detail });
        let element = document.getElementById(id);
        if (element == null) throw "No element found."
        element.dispatchEvent(_event);
    }
}
