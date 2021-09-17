using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ZemotoUtils
{
   public sealed class ProcessCpuMonitor : IDisposable
   {
      private static readonly PerformanceCounter TotalCpuUsageCounter;
      private readonly PerformanceCounter _cpuUsageCounter;

      static ProcessCpuMonitor()
      {
         TotalCpuUsageCounter = new PerformanceCounter( "Processor Information", "% Processor Time", "_Total", true );
         TotalCpuUsageCounter.NextValue();
      }

      public ProcessCpuMonitor( Process process )
      {
         _cpuUsageCounter = new PerformanceCounter( "Process", "% Processor Time", GetInstanceName( process ), true );
         _cpuUsageCounter.NextValue();
      }

      public void Dispose()
      {
         _cpuUsageCounter?.Dispose();
      }

      public int GetCpuUsage()
      {
         try
         {
            return (int)( _cpuUsageCounter.NextValue() / Environment.ProcessorCount );
         }
         catch ( InvalidOperationException )
         {
            return 0;
         }
      }

      public static int GetTotalCpuUsage() => (int)TotalCpuUsageCounter.NextValue();

      private static string GetInstanceName( Process process )
      {
         string processName = Path.GetFileNameWithoutExtension( process.ProcessName );

         var vategory = new PerformanceCounterCategory( "Process" );
         var instances = vategory.GetInstanceNames().Where( x => x.StartsWith( processName ) );

         foreach ( string instance in instances )
         {
            using ( PerformanceCounter cnt = new PerformanceCounter( "Process", "ID Process", instance, true ) )
            {
               int val = (int)cnt.RawValue;
               if ( val == process.Id )
               {
                  return instance;
               }
            }
         }
         return null;
      }
   }
}
