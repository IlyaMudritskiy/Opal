using Opal.Model.AppConfiguration;
using ProcessDashboard.src.CommonClasses.SreenProvider;
using System;
using System.Collections.Generic;

namespace Opal.src.CommonClasses.SreenProvider
{
    public static class ScreenFactory
    {
        private static readonly Dictionary<string, IScreenCreator<IScreen>> _creators = new Dictionary<string, IScreenCreator<IScreen>>();

        static ScreenFactory()
        {
            _creators.Add(new TTLScreenCreator().Name, new TTLScreenCreator());
        }

        public static IScreen Create(string lineId)
        {
            if (_creators.TryGetValue(lineId, out var creator))
            {
                return creator.Create();
            }
            throw new ArgumentException($"Unknown screen type: {lineId}", nameof(creator));
        }
    }
}
