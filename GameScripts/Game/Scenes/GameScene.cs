using GameEngine.Components.Core;
using GameEngine.Components.UI;
using GameEngine.Services;
using GameEngine.Systems;
using LurkerCommand.GameSystem;
using LurkerCommand.MapSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.Scenes
{
    public sealed class GameScene : Scene
    {
        private readonly GraphicsDevice _device;
        public GameScene(GraphicsDevice device) => _device = device;
        private static Text _timeText;
        private static Button _skipMoveButton;

        private const string timeLeftMessage = "Time Left: ";
        public static void UpdateTime(int time)
        {
            if(time >= 40) {
                _timeText.Color = Color.Green;
            }
            else if(time < 40 && time > 10) {
                _timeText.Color = Color.Yellow;
            }
            else {
                _timeText.Color = Color.Red;
            }
            _timeText.text = timeLeftMessage + time.ToString() + "s";
        }
        public override void Load()
        {
            TeamManager.Init();
            Field.SetMap(this);

            Camera2D cm = new Camera2D(_device);
            SetCamera(cm);

            CameraMovement cmMovement = new CameraMovement(cm, new Vector2(Field.MapWidth / 2, Field.MapHeight / 2));
            Add(cmMovement);

            var spawnData = Field.GetBaseSpawnPoints();

            foreach (var data in spawnData)
            {
                Unit newUnit = new Unit();
                newUnit.Setup(AssetManager.GetFont("Arial"), data.position, 3);
                TeamManager.AddUnitToTeam(data.team, newUnit);
                Add(newUnit);

                Field.GetCell(data.position).BindUnit(newUnit);
            }
            TeamManager.CurrentTeam.RefreshTurn();

            float screenWidth = _device.Viewport.Width;
            float screenHeight = _device.Viewport.Height;

            SpriteFont font = AssetManager.GetFont("Pixel");
            Texture2D buttonTexture = AssetManager.GetTexture("rectangle-hexagon");

            Vector2 textPosition = new Vector2(screenWidth / 2, 20);
            _timeText = new Text(font, timeLeftMessage + "00", textPosition, Color.Green);
            _timeText.Transform.LocalScale = new Vector2(0.5f, 0.5f);

            float buttonWidth = buttonTexture.Width;
            Vector2 buttonPosition = new Vector2(screenWidth - buttonWidth - 40, 20);

            _skipMoveButton = new Button(buttonTexture, buttonPosition, new Vector2(0.1f, 0.1f), Color.White, font, "Skip Move");
            _skipMoveButton.text.Transform.LocalScale = new Vector2(0.2f, 0.2f); 
            _skipMoveButton.onClicked += TeamManager.NextTurn;

            AddUI(_timeText);
            AddUI(_skipMoveButton);
        }

        public override void Update(GameTime gameTime)
        {
            HandleGameInput();
            TeamManager.Update(gameTime);
            base.Update(gameTime);
        }

        private void HandleGameInput()
        {
            Vector2 mouseWorld = camera.ScreenToWorld(InputManager.MousePosition);
            Cell mouseCell = Field.GetCellByWorldPos(mouseWorld);
        }
    }
}