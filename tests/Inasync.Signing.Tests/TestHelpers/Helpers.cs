using System.Collections.Generic;
using System.Diagnostics;

namespace System {

    public static class ActionEnumerableExtensions {

        public static void Run(this IEnumerable<Action> actions) {
            Debug.Assert(actions != null);

            foreach (var action in actions) {
                action();
            }
        }
    }
}
