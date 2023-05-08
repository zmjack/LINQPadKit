/// <reference path="./Drawing.ts" />

class DrawingOptions {
    strokeColor?: string;
    fillColor?: string;
}

abstract class HybridCanvas {
    public abstract classNames: {
        graph: string;
        sprites_container: string;
        sprite: string;
    };

    element: HTMLElement;
    element_sprites_container: HTMLElement;
    element_sprites: HTMLElement[] = [];
    element_canvas: HTMLCanvasElement;
    context2D: CanvasRenderingContext2D;

    set size(value: Size) {
        this.element_canvas.width = value.width;
        this.element_canvas.height = value.height;
    }

    abstract refresh(...params: any[]): void;

    render(element: HTMLElement, ...params: any[]): void {
        this.element = element;
        this.element.className = this.classNames.graph;

        this.element_sprites_container = this.element.getElementsByClassName(this.classNames.sprites_container)[0] as HTMLElement;
        if (this.element_sprites_container == null) {
            let container = document.createElement('div');
            container.className = this.classNames.sprites_container;
            this.element.appendChild(container);
            this.element_sprites_container = container;
        }

        this.element_canvas = this.element.getElementsByTagName(`canvas`)[0] as HTMLCanvasElement;
        if (this.element_canvas == null) {
            let canvas = document.createElement('canvas');
            this.element.appendChild(canvas);
            this.element_canvas = canvas;
        }

        this.context2D = this.element_canvas.getContext('2d') as CanvasRenderingContext2D;
        this.refresh(...params);
    }

    clear(): void {
        this.clearCanvas();
        this.clearSprites();
    }

    clearCanvas(): void {
        this.context2D.clearRect(0, 0, this.element_canvas.width, this.element_canvas.height);
    }

    addSprite(sprite: HTMLElement, exsist: boolean): void {
        this.element_sprites.push(sprite);
        if (!exsist) this.element_sprites_container.appendChild(sprite);
    }

    clearSprites(): void {
        this.element_sprites_container.innerHTML = '';
        this.element_sprites = [];
    }

    useOptions(options: DrawingOptions): void {
        if (options?.strokeColor) {
            this.context2D.strokeStyle = options.strokeColor;
        }
        if (options?.fillColor) {
            this.context2D.fillStyle = options.fillColor;
        }
    }

    polygon(
        positions: Position2D[],
        options?: DrawingOptions
    ): void {
        let first = true;
        if (positions.length > 0) {
            this.context2D.beginPath();
            for (let pos of positions) {
                if (first) {
                    this.context2D.moveTo(pos.x, pos.y);
                    first = false;
                }
                else this.context2D.lineTo(pos.x, pos.y);
            }
            this.context2D.closePath();
            this.useOptions(options);
            this.context2D.stroke();
            this.context2D.fill();
        }
    }

    line(from: Position2D, to: Position2D, options?: DrawingOptions): void {
        this.context2D.beginPath();
        this.context2D.moveTo(from.x, from.y);
        this.context2D.lineTo(to.x, to.y);
        this.context2D.closePath();

        this.useOptions(options);
        this.context2D.stroke();
    }

    rectangle(lt: Position2D, rb: Position2D, options?: DrawingOptions): void {

        this.polygon([lt, { x: rb.x, y: lt.y }, rb, { x: lt.x, y: rb.y }], options);
    }
}
