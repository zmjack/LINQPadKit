/// <reference path="../fabric-impl.d.ts" />
type Shape = 'rectangle' | 'hexagon';
type FabricShape = fabric.Rect | fabric.Path;

interface TileMapCell {
    element: FabricShape;
    x: number;
    y: number;
}

class TileMap {
    bridge: string;
    cells: TileMapCell[][];
    canvas: HTMLCanvasElement;
    fabricCanvas: fabric.Canvas;

    constructor(public shape: Shape, public size: number, public width: number, public height: number) {

    }

    render(id: string, params?: { bridge?: string }): void {
        if (params?.bridge) {
            let el_bridge = document.getElementById(params.bridge);
            if (el_bridge == null) throw "Invalid bridge id.";
            this.bridge = params.bridge;
        }

        let element = document.getElementById(id);
        element.innerHTML = '';

        let canvas = document.createElement('canvas');

        if (this.shape == 'rectangle') {
            canvas.width = this.size * this.width;
            canvas.height = this.size * this.height;
        } else if (this.shape == 'hexagon') {
            canvas.width = this.size * this.width + this.size / 2;
            canvas.height = (this.size / 2 * Math.sqrt(3) / 2) * (this.height * 2 + 1);
        }
        element.appendChild(canvas);
        this.canvas = canvas;

        this.fabricCanvas = new fabric.Canvas(canvas);
        this.fabricCanvas.selection = false;
        this.init(this.shape);
    }

    setImage(x: number, y: number, color: string) {
        let cell = this.cells[y][x];
        (cell.element as any).set('fill', color);
        this.fabricCanvas.renderAll();
    }

    setColor(x: number, y: number, color: string) {
        let cell = this.cells[y][x];
        (cell.element as any).set('fill', color);
        this.fabricCanvas.renderAll();
    }

    getColor(x: number, y: number): string {
        let cell = this.cells[y][x];
        return (cell.element as any).get('fill');
    }

    resetColor(x: number, y: number) {
        let cell = this.cells[y][x];
        (cell.element as any).set('fill', this.getOriginColor(x, y));
        this.fabricCanvas.renderAll();
    }

    getOriginColor(x: number, y: number) {
        return '#C0C0C0';
    }

    init(shape: Shape) {
        this.cells = [];
        if (shape == 'rectangle') {
            for (let y = 0; y < this.height; y++) {
                let row: TileMapCell[] = [];
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

                    let cell = { element: rect, y, x };
                    rect.on('mousedown', e => {
                        this.mousedown(this.bridge, 'cell:mousedown', { x: cell.x, y: cell.y });
                    });
                    this.fabricCanvas.add(rect);
                    row.push(cell);
                }
                this.cells.push(row);
            }
        } else if (shape == 'hexagon') {
            for (let y = 0; y < this.height; y++) {
                let row: TileMapCell[] = [];
                for (let x = 0; x < this.width; x++) {
                    let height_d2 = this.size / 2 * Math.sqrt(3) / 2;
                    let left = x * this.size + (y % 2 == 0 ? 0 : this.size / 2);
                    let top = y * this.size / 2 * Math.sqrt(3);
                    let points: [number, number][] = [
                        [left + Math.floor(this.size / 2), top],
                        [left + this.size, top + height_d2],
                        [left + this.size, top + height_d2 * 2],
                        [left + Math.floor(this.size / 2), top + height_d2 * 3],
                        [left, top + height_d2 * 2],
                        [left, top + height_d2],
                    ];

                    let path = new fabric.Path(`M ${points.map(p => `${p[0]} ${p[1]}`).join(' L ')} z`);
                    path.fill = this.getOriginColor(y, x);
                    path.selectable = false;
                    path.hoverCursor = 'default';

                    let cell = { element: path, y, x };
                    path.on('mousedown', e => {
                        this.mousedown(this.bridge, 'cell:mousedown', { x: cell.x, y: cell.y });
                    });
                    this.fabricCanvas.add(path);
                    row.push(cell);
                }
                this.cells.push(row);
            }
        }
    }

    mousedown(id: string, event: string, detail: object) {
        let _event = new CustomEvent(event, { detail });
        let element = document.getElementById(id);
        element.dispatchEvent(_event);
    }
}
