let $console = {
    element: document.getElementById('console'),
    log: function (s: string) {
        let p = document.createElement('p');
        p.innerText = s;
        (this.element as HTMLElement).appendChild(p);
    },
    text: function (s: string) {
        let p = document.createElement('p');
        p.innerText = s;
        (this.element as HTMLElement).innerHTML = null;
        (this.element as HTMLElement).appendChild(p);
    }
}

let tailMap_rectangle = new TileMap('rectangle', 50, 8, 7);
tailMap_rectangle.render('rectangle', { bridge: 'rectangle_bridge' });

document.getElementById('rectangle_bridge').addEventListener('cell:mousedown', (args: CustomEvent) => {
    let detail: { x: number, y: number } = args.detail;
    let x = detail.x;
    let y = detail.y;

    let color = (document.getElementById('color') as HTMLInputElement).value;

    if (tailMap_rectangle.getColor(x, y) == color) {
        tailMap_rectangle.resetColor(x, y);
    }
    else tailMap_rectangle.setColor(x, y, color);
});

let tailMap_hexagon = new TileMap('hexagon', 50, 8, 7);
tailMap_hexagon.render('hexagon', { bridge: 'hexagon_bridge' });

document.getElementById('hexagon_bridge').addEventListener('cell:mousedown', (args: CustomEvent) => {
    let detail: { x: number, y: number } = args.detail;
    let x = detail.x;
    let y = detail.y;

    let color = (document.getElementById('color') as HTMLInputElement).value;

    $console.log(`x: ${x}, y: ${y}`);

    if (tailMap_hexagon.getColor(x, y) == color) {
        tailMap_hexagon.resetColor(x, y);
    }
    else tailMap_hexagon.setColor(x, y, color);
});

