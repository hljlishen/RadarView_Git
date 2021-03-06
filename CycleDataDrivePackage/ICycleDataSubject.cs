﻿using System;

namespace CycleDataDrivePackage
{
    public interface ICycleDataSubject : IDisposable
    {
        void RegisterObserver(ICycleDataObserver ob);

        void UnregisterObserver(ICycleDataObserver ob);

        void RebindSource(string source);

        void StartReading();

        void SpeedUp();

        void SpeedDown();

        void Pause();

        void Resume();
    }
}
