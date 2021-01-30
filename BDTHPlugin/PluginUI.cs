using ImGuiNET;
using System;
using System.Numerics;

namespace BDTHPlugin
{
    // It is good to have this be disposable in general, in case you ever need it
    // to do any cleanup
    class PluginUI : IDisposable
    {
        private readonly Configuration configuration;
        private readonly PluginMemory memory;

        // this extra bool exists for ImGui, since you can't ref a property
        private bool visible = false;
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        private float drag = 0.05f;
        private bool placeAnywhere = false;
        private readonly Vector4 orangeColor = new Vector4(0.871f, 0.518f, 0f, 1f);

        public PluginUI(Configuration configuration, PluginMemory memory)
        {
            this.configuration = configuration;
            this.memory = memory;
        }

        public void Dispose()
        {
        }

        public void Draw()
        {
            DrawMainWindow();
        }

        public void DrawMainWindow()
        {
            if (!Visible)
            {
                return;
            }

            ImGui.PushStyleColor(ImGuiCol.TitleBgActive, orangeColor);
            ImGui.PushStyleColor(ImGuiCol.CheckMark, orangeColor);

            var scale = ImGui.GetIO().FontGlobalScale;
            var size = new Vector2(380 * scale, 260 * scale);

            ImGui.SetNextWindowSize(size, ImGuiCond.Always);
            ImGui.SetNextWindowSizeConstraints(size, size);

            if (ImGui.Begin("Burning Down the House", ref this.visible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoResize))
            {
                ImGui.BeginGroup();

                if (ImGui.Checkbox("Place Anywhere", ref this.placeAnywhere))
                {
                    // Set the place anywhere based on the checkbox state.
                    this.memory.SetPlaceAnywhere(this.placeAnywhere);
                }

                // Disabled if the housing mode isn't on and there isn't a selected item.
                var disabled = !(this.memory.IsHousingModeOn() && this.memory.selectedItem != IntPtr.Zero);

                // Set the opacity based on if housing is on.
                if (disabled)
                    ImGui.PushStyleVar(ImGuiStyleVar.Alpha, .3f);

                ImGui.PushItemWidth(73f);

                if (ImGui.DragFloat("##xdrag", ref this.memory.position.X, this.drag))
                    memory.WritePosition(this.memory.position);
                ImGui.SameLine(0, 4);
                var xHover = ImGui.IsMouseHoveringRect(ImGui.GetItemRectMin(), ImGui.GetItemRectMax());

                if (ImGui.DragFloat("##ydrag", ref this.memory.position.Y, this.drag))
                    memory.WritePosition(this.memory.position);
                ImGui.SameLine(0, 4);
                var yHover = ImGui.IsMouseHoveringRect(ImGui.GetItemRectMin(), ImGui.GetItemRectMax());

                if (ImGui.DragFloat("##zdrag", ref this.memory.position.Z, this.drag))
                    memory.WritePosition(this.memory.position);
                ImGui.SameLine(0, 4);
                var zHover = ImGui.IsMouseHoveringRect(ImGui.GetItemRectMin(), ImGui.GetItemRectMax());

                if (ImGui.DragInt("##angledrag", ref this.memory.angle, 1, -180, 180))
                {
                    memory.WriteRotation(this.memory.angle);
                }
                ImGui.SameLine(0, 4);
                var angleHover = ImGui.IsMouseHoveringRect(ImGui.GetItemRectMin(), ImGui.GetItemRectMax());

                ImGui.PopItemWidth();

                ImGui.Text("position");

                // Mouse wheel direction.
                var delta = ImGui.GetIO().MouseWheel * this.drag;

                // Move position based on which control is being hovered.
                if (xHover)
                    this.memory.position.X += delta;
                if (yHover)
                    this.memory.position.Y += delta;
                if (zHover)
                    this.memory.position.Z += delta;
                if (angleHover && Math.Abs(delta) > 0)
                    this.memory.angle += delta > 0 ? 1 : -1;
                if (xHover || yHover || zHover)
                    memory.WritePosition(this.memory.position);
                if (angleHover)
                    memory.WriteRotation(this.memory.angle);

                if (ImGui.InputFloat("x coord", ref this.memory.position.X, this.drag))
                    memory.WritePosition(this.memory.position);
                xHover = ImGui.IsMouseHoveringRect(ImGui.GetItemRectMin(), ImGui.GetItemRectMax());

                if (ImGui.InputFloat("y coord", ref this.memory.position.Y, this.drag))
                    memory.WritePosition(this.memory.position);
                yHover = ImGui.IsMouseHoveringRect(ImGui.GetItemRectMin(), ImGui.GetItemRectMax());

                if (ImGui.InputFloat("z coord", ref this.memory.position.Z, this.drag))
                    memory.WritePosition(this.memory.position);
                zHover = ImGui.IsMouseHoveringRect(ImGui.GetItemRectMin(), ImGui.GetItemRectMax());


                if (ImGui.InputInt("angle", ref this.memory.angle, 1, 10))
                    memory.WriteRotation(this.memory.angle);
                angleHover = ImGui.IsMouseHoveringRect(ImGui.GetItemRectMin(), ImGui.GetItemRectMax());

                // Mouse wheel direction.
                delta = ImGui.GetIO().MouseWheel * this.drag;

                // Move position based on which control is being hovered.
                if (xHover)
                    this.memory.position.X += delta;
                if (yHover)
                    this.memory.position.Y += delta;
                if (zHover)
                    this.memory.position.Z += delta;
                if (angleHover && Math.Abs(delta) > 0)
                    this.memory.angle += delta > 0 ? 1 : -1;
                if (xHover || yHover || zHover)
                    memory.WritePosition(this.memory.position);
                if (angleHover)
                    memory.WriteRotation(this.memory.angle);

                ImGui.NewLine();

                if (disabled)
                    ImGui.PopStyleVar();

                // Drag ammount for the inputs.
                ImGui.InputFloat("drag", ref this.drag, 0.05f);
            }
            ImGui.End();

            ImGui.PopStyleColor(2);
        }
    }
}
