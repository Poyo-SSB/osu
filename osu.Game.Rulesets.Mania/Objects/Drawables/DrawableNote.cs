﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Extensions.Color4Extensions;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Mania.Judgements;
using osu.Game.Rulesets.Mania.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Mania.Objects.Drawables
{
    /// <summary>
    /// Visualises a <see cref="Note"/> hit object.
    /// </summary>
    public class DrawableNote : DrawableManiaHitObject<Note>, IKeyBindingHandler<ManiaAction>
    {
        private readonly NotePiece headPiece;

        public DrawableNote(Note hitObject)
            : base(hitObject)
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            CornerRadius = 5;
            Masking = true;

            InternalChild = headPiece = new NotePiece();
        }

        protected override void OnDirectionChanged(ScrollingDirection direction)
        {
            base.OnDirectionChanged(direction);

            headPiece.Anchor = headPiece.Origin = direction == ScrollingDirection.Up ? Anchor.TopCentre : Anchor.BottomCentre;
        }

        public override Color4 AccentColour
        {
            get { return base.AccentColour; }
            set
            {
                base.AccentColour = value;
                headPiece.AccentColour = AccentColour;

                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Glow,
                    Colour = AccentColour.Lighten(1f).Opacity(0.6f),
                    Radius = 10,
                };
            }
        }

        protected override void CheckForJudgements(bool userTriggered, double timeOffset)
        {
            if (!userTriggered)
            {
                if (!HitObject.HitWindows.CanBeHit(timeOffset))
                    AddJudgement(new ManiaJudgement { Result = HitResult.Miss });
                return;
            }

            var result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None)
                return;

            AddJudgement(new ManiaJudgement { Result = result });
        }

        public virtual bool OnPressed(ManiaAction action)
        {
            if (action != Action.Value)
                return false;

            return UpdateJudgement(true);
        }

        public virtual bool OnReleased(ManiaAction action) => false;
    }
}
