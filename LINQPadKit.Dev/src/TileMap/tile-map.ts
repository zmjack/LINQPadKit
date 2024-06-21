/// <reference path="./fabric-impl.d.ts" />
type Shape = 'rectangle' | 'hexagon';

interface Cell {
    element: fabric.Object;
    x: number;
    y: number;
    color?: string;
    image?: string;
}

class TileMap {
    net_bridge?: string;
    canvas?: HTMLCanvasElement;
    fabricCanvas?: fabric.Canvas;
    cells: Cell[][] = [];
    cellHeight: number;

    onCellMouseDown: ((sender: TileMap, args: { x: number, y: number }) => void) | null = null;
    onCellColorChange: ((sender: TileMap, args: { color: string }) => void) | null = null;
    onImageChange: ((sender: TileMap, args: { image: string }) => void) | null = null;
    onReady: ((sender: TileMap, args: { rows: number, cols: number }) => void) | null = null;

    constructor(public shape: Shape, public cellWidth: number, public rows: number, public cols: number) {
        this.cellHeight = this.cellWidth / 2 * Math.sqrt(3) / 2 * 3;
    }

    render(id: string, net_bridge?: string) {
        this.net_bridge = net_bridge;

        const element = document.getElementById(id);
        if (element == null) throw "No element found.";

        element.innerHTML = '';
        const canvas = document.createElement('canvas');
        if (canvas == null) throw "No canvas found.";

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
    }

    private init(shape) {
        if (this.fabricCanvas == null) throw "No fabric canvas found."

        this.cells = [];
        if (shape == 'rectangle') {
            for (let y = 0; y < this.rows; y++) {
                const row: Cell[] = [];
                for (let x = 0; x < this.cols; x++) {
                    const rect = new fabric.Rect({
                        left: x * this.cellWidth,
                        top: y * this.cellWidth,
                        fill: this.getOriginColor(y, x),
                        width: this.cellWidth,
                        height: this.cellWidth,
                        selectable: false,
                        hoverCursor: 'default'
                    });
                    const cell: Cell = { element: rect, y: y, x: x };
                    rect.on('mousedown', e => {
                        this.mouseDown({ x: cell.x, y: cell.y });
                    });
                    this.fabricCanvas.add(rect);
                    row.push(cell);
                }
                this.cells.push(row);
            }
        }
        else if (shape == 'hexagon') {
            for (let y = 0; y < this.rows; y++) {
                const row: Cell[] = [];
                for (let x = 0; x < this.cols; x++) {
                    const height = this.cellWidth / 2 * Math.sqrt(3) / 2;
                    const left = x * this.cellWidth + (y % 2 == 0 ? 0 : this.cellWidth / 2);
                    const top = y * this.cellWidth / 2 * Math.sqrt(3);
                    const points = [
                        [left + Math.floor(this.cellWidth / 2), top],
                        [left + this.cellWidth, top + height],
                        [left + this.cellWidth, top + height * 2],
                        [left + Math.floor(this.cellWidth / 2), top + height * 3],
                        [left, top + height * 2],
                        [left, top + height],
                    ];

                    const path = new fabric.Path(`M ${points.map(p => `${p[0]} ${p[1]}`).join(' L ')} z`);
                    path.fill = this.getOriginColor(y, x);
                    path.selectable = false;
                    path.hoverCursor = 'default';

                    const cell: Cell = {
                        element: path,
                        y, x,
                        color: path.fill,
                    };
                    path.on('mousedown', e => {
                        this.mouseDown({ x: cell.x, y: cell.y });
                    });
                    this.fabricCanvas.add(path);
                    row.push(cell);
                }
                this.cells.push(row);
            }
        }
    };

    setImage(x, y, image: string) {
        var img = new Image();
        img.src = image;
        img.onload = () => {
            if (this.fabricCanvas == null) throw "No fabric canvas found."

            var pattern = new fabric.Pattern({
                source: img,
                repeat: 'no-repeat',
                offsetX: (this.cellWidth / 2) - (img.width / 2),
                offsetY: (this.cellHeight / 2) - (img.height / 2)
            });

            const cell = this.cells[y][x];
            cell.color = undefined;
            cell.image = image;
            cell.element.set('fill', pattern);
            this.fabricCanvas.renderAll();
            this.imageChange?.({ x, y, image });
        };
    }

    getImage(x, y) {
        const cell = this.cells[y][x];
        return cell.image;
    }

    setColor(x, y, color) {
        if (this.fabricCanvas == null) throw "No fabric canvas found."

        const cell = this.cells[y][x];
        cell.image = undefined;
        cell.color = color;
        cell.element.set('fill', color);
        this.fabricCanvas.renderAll();
        this.colorChange?.({ x, y, color });
    }

    getColor(x, y) {
        const cell = this.cells[y][x];
        return cell.color;
    }

    resetCell(x, y) {
        const color = this.getOriginColor(x, y);
        this.setColor(x, y, color);
    }

    getOriginColor(x, y) {
        return '#C0C0C0';
    }

    private callBridge(name: string, detail) {
        if (this.net_bridge) {
            const element = document.getElementById(this.net_bridge);
            if (element == null) throw "No element found."
            const _event = new CustomEvent(name, { detail });
            element.dispatchEvent(_event);
        }
    }

    private mouseDown(detail: { x: number, y: number }) {
        this.onCellMouseDown?.(this, detail);
        this.callBridge('cell:mouseDown', detail);
    }

    private colorChange(detail: { x: number, y: number, color: string }) {
        this.onCellColorChange?.(this, detail);
        this.callBridge('cell:colorChange', detail);
    }

    private imageChange(detail: { x: number, y: number, image: string }) {
        this.onImageChange?.(this, detail);
        this.callBridge('cell:imageChange', detail);
    }

    private ready(detail: { rows: number, cols: number }) {
        this.onReady?.(this, detail);
        this.callBridge('ready', detail);
    }
}
