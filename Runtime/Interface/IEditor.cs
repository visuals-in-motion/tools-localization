using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals
{
    public interface IEditor
    {
        bool localizationEnable { get; set; }
        string localizationType { get; }
        int localizationCategory { get; set; }
        int localizationKey { get; set; }
        void LocalizationLoad();
        void LocalizationChange(int index);
    }
}
