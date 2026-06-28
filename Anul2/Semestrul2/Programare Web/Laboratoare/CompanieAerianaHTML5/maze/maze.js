class MazeGame {
    constructor(width = 21, height = 21, cellSize = 20) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.maze = [];
        this.playerX = 1;
        this.playerY = 1;
        this.exitX = width - 2;
        this.exitY = height - 2;
        this.gameWon = false;
        this.$canvas = $('#mazeCanvas');
        this.$statusDisplay = $('#status');

        this.generateMaze();
        this.setupEventListeners();
        this.render();
    }

    // generarea unui nou labirint folosind DFS
    generateMaze() {
        this.gameWon = false;
        this.playerX = 1;
        this.playerY = 1;

        // initializarea labirintului cu "peretii"
        this.maze = [];
        for (let y = 0; y < this.height; y++) {
            this.maze[y] = [];
            for (let x = 0; x < this.width; x++) {
                this.maze[y][x] = 1; // 1 = wall
            }
        }

        // formarea path-ului (aici se foloseste dfs)
        this.carvePath(1, 1);

        
        this.maze[1][1] = 0; // Start
        this.maze[this.exitY][this.exitX] = 0; // Exit
        this.$statusDisplay.text('Un nou labirint a fost generat! Navighează spre ieșire.');
        this.$statusDisplay.attr('class', 'status-message');
    }

    // DFS pentru determinarea path-ului
    carvePath(x, y) {
        this.maze[y][x] = 0; 

        // setarea directiilor
        const directions = [[0, 2], [2, 0], [0, -2], [-2, 0]];
        const shuffled = directions.sort(() => Math.random() - 0.5);

        for (const [dx, dy] of shuffled) {
            const nx = x + dx;
            const ny = y + dy;

            // verifica marginile (bounds)
            if (nx > 0 && nx < this.width - 1 && ny > 0 && ny < this.height - 1) {
                if (this.maze[ny][nx] === 1) {
                    this.maze[y + dy / 2][x + dx / 2] = 0;
                    this.carvePath(nx, ny);
                }
            }
        }
    }

    // player movement
    movePlayer(dx, dy) {
        if (this.gameWon) return;

        const newX = this.playerX + dx;
        const newY = this.playerY + dy;

        // caz de verificare a bounds + coliziuni
        if (newX > 0 && newX < this.width - 1 && 
            newY > 0 && newY < this.height - 1 && 
            this.maze[newY][newX] === 0) {
            
            this.playerX = newX;
            this.playerY = newY;

            // caz in care ajunge la iesire
            if (this.playerX === this.exitX && this.playerY === this.exitY) {
                this.gameWon = true;
                this.$statusDisplay.text('Ai găsit ieșirea! Generează un nou labirint pentru a juca din nou.');
                this.$statusDisplay.attr('class', 'status-message win');
            }

            this.render();
        }
    }


    render() {
        this.$canvas.empty();

        const canvasWidth = this.width * this.cellSize;
        const canvasHeight = this.height * this.cellSize;

        // creaza un svg
        const svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        svg.setAttribute('width', canvasWidth);
        svg.setAttribute('height', canvasHeight);
        svg.setAttribute('viewBox', `0 0 ${canvasWidth} ${canvasHeight}`);

        // deseneaza efectiv labirintul
        for (let y = 0; y < this.height; y++) {
            for (let x = 0; x < this.width; x++) {
                const rect = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
                rect.setAttribute('x', x * this.cellSize);
                rect.setAttribute('y', y * this.cellSize);
                rect.setAttribute('width', this.cellSize);
                rect.setAttribute('height', this.cellSize);

                if (this.maze[y][x] === 1) {
                    // wall
                    rect.setAttribute('fill', '#1d2a38');
                } else {
                    // path
                    rect.setAttribute('fill', '#f0f0f0');
                }
                rect.setAttribute('stroke', '#e0e0e0');
                rect.setAttribute('stroke-width', '0.5');

                svg.appendChild(rect);
            }
        }

        // exit
        const exitCircle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
        exitCircle.setAttribute('cx', this.exitX * this.cellSize + this.cellSize / 2);
        exitCircle.setAttribute('cy', this.exitY * this.cellSize + this.cellSize / 2);
        exitCircle.setAttribute('r', this.cellSize / 2.5);
        exitCircle.setAttribute('fill', '#4CAF50');
        svg.appendChild(exitCircle);

        // player 
        const playerText = document.createElementNS('http://www.w3.org/2000/svg', 'text');
        playerText.setAttribute('x', this.playerX * this.cellSize + this.cellSize / 2);
        playerText.setAttribute('y', this.playerY * this.cellSize + this.cellSize / 2);
        playerText.setAttribute('font-size', this.cellSize * 1.2);
        playerText.setAttribute('text-anchor', 'middle');
        playerText.setAttribute('dominant-baseline', 'central');
        playerText.textContent = '✈️';
        svg.appendChild(playerText);

        this.$canvas.append(svg);
    }

    setupEventListeners() {
        // keyboard controls
        $(document).on('keydown', (e) => {
            switch(e.key) {
                case 'ArrowUp':
                    e.preventDefault();
                    this.movePlayer(0, -1);
                    break;
                case 'ArrowDown':
                    e.preventDefault();
                    this.movePlayer(0, 1);
                    break;
                case 'ArrowLeft':
                    e.preventDefault();
                    this.movePlayer(-1, 0);
                    break;
                case 'ArrowRight':
                    e.preventDefault();
                    this.movePlayer(1, 0);
                    break;
            }
        });

        // button controls
        $('#generateBtn').on('click', () => {
            this.generateMaze();
            this.render();
        });

        $('#resetBtn').on('click', () => {
            this.gameWon = false;
            this.playerX = 1;
            this.playerY = 1;
            this.$statusDisplay.text('Position reset. Navigate to the exit.');
            this.$statusDisplay.attr('class', 'status-message');
            this.render();
        });
    }
}

// initializare
$(document).ready(() => {
    new MazeGame(21, 21, 20);
});
