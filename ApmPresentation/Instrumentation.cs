﻿using System.Diagnostics;

namespace ApmPresentation
{
    public class Instrumentation : IDisposable
    {
        public static readonly string ApmPresentationService = "MainApp";

        public Instrumentation()
        {
            ActivitySource = new(ApmPresentationService);
        }

        public ActivitySource ActivitySource { get; }

        public void Dispose()
        {
            ActivitySource?.Dispose();
        }
    }
}
