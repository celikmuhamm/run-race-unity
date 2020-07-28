using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatform
{
  GameObject GetGameObj(PlatformConfig config,GameObject last,float levelMultiplier);
}
