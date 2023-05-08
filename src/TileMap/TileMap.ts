/// <reference path="../Drawing.ts" />
/// <reference path="../HybridCanvas.ts" />

class TileMap extends HybridCanvas {
    private tileHtmlMap: { [key: string]: string };

    public override classNames = {
        graph: 'tile-map',
        sprites_container: 'tile-map-sprites',
        sprite: 'tile-map-sprite',
    };

    protected constructor(private tileSize: number, private tileWidth: number, private tileHeight: number) {
        super();
    }

    override render(element: HTMLElement, ...params: any[]): void {
        super.render(element, ...params);

        let sprites = this.element_sprites_container.getElementsByClassName(this.classNames.sprite);
        for (let i = 0; i < sprites.length; i++) {
            var sprite = sprites[i] as HTMLElement;
            this.addSprite(sprite, true);
        }

        if (this.element_sprites.length != this.tileHeight * this.tileWidth) throw `The length(${this.element_sprites.length}) of sprites must be ${this.tileHeight * this.tileWidth}.`;
    }

    override refresh() {
        if (this.element == null) throw "Use render before refresh."

        this.clearCanvas();
        this.size = {
            width: this.tileSize * this.tileWidth,
            height: this.tileSize * this.tileHeight,
        };
        this.drawBackground();
    }

    private drawBackground() {
        for (let y = 0; y < this.tileHeight; y++) {
            for (let x = 0; x < this.tileWidth; x++) {
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
    }

    useMap(tileHtmlMap: { [key: number]: string }) {
        this.tileHtmlMap = tileHtmlMap;
    }

    loadMap(map: number[][]) {
        if (this.element_sprites.length == 0) {
            for (let y = 0; y < this.tileHeight; y++) {
                for (let x = 0; x < this.tileWidth; x++) {
                    this.addSprite(this.createSprite(), false);
                }
            }
        }

        if (this.element_sprites.length != this.tileHeight * this.tileWidth) throw `The length(${this.element_sprites.length}) of sprites must be ${this.tileHeight * this.tileWidth}.`;

        for (let y = 0; y < this.tileHeight; y++) {
            for (let x = 0; x < this.tileWidth; x++) {
                let div = this.element_sprites[y * this.tileHeight + x];
                let value = map[y] ? map[y][x] : undefined;
                div.innerHTML = value ? this.tileHtmlMap[value] : '';
                div.style['top'] = `${y * this.tileSize}px`;
                div.style['left'] = `${x * this.tileSize}px`;
                div.style['width'] = `${this.tileSize}px`;
                div.style['height'] = `${this.tileSize}px`;
            }
        }
    }

    createSprite() {
        let div = document.createElement('div');
        div.className = this.classNames.sprite;
        return div;
    }
}
