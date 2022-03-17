using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals
{
    public interface IEditor
    {
        bool localizationEnable { get; set; }
        string localizationType { get; }
        string localizationCategory { get; set; }
        string localizationKeyName { get; set; }
        void LocalizationLoad();
        void LocalizationChange(int index, bool changeValues);
    }
}
