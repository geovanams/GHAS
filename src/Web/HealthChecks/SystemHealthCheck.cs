using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.eShopWeb.Web.HealthChecks;

public class SystemHealthCheck : IHealthCheck
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SystemHealthCheck(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        string drive = request.Query.ContainsKey("drive") ? request.Query["drive"].ToString().ToUpper() : "C";
        var allowedDrives = new Dictionary<string, string>
        {
            { "A", "/C fsutil volume diskfree A:" },
            { "B", "/C fsutil volume diskfree B:" },
            { "C", "/C fsutil volume diskfree C:" },
            { "D", "/C fsutil volume diskfree D:" },
            { "E", "/C fsutil volume diskfree E:" },
            { "F", "/C fsutil volume diskfree F:" },
            { "G", "/C fsutil volume diskfree G:" },
            { "H", "/C fsutil volume diskfree H:" },
            { "I", "/C fsutil volume diskfree I:" },
            { "J", "/C fsutil volume diskfree J:" },
            { "K", "/C fsutil volume diskfree K:" },
            { "L", "/C fsutil volume diskfree L:" },
            { "M", "/C fsutil volume diskfree M:" },
            { "N", "/C fsutil volume diskfree N:" },
            { "O", "/C fsutil volume diskfree O:" },
            { "P", "/C fsutil volume diskfree P:" },
            { "Q", "/C fsutil volume diskfree Q:" },
            { "R", "/C fsutil volume diskfree R:" },
            { "S", "/C fsutil volume diskfree S:" },
            { "T", "/C fsutil volume diskfree T:" },
            { "U", "/C fsutil volume diskfree U:" },
            { "V", "/C fsutil volume diskfree V:" },
            { "W", "/C fsutil volume diskfree W:" },
            { "X", "/C fsutil volume diskfree X:" },
            { "Y", "/C fsutil volume diskfree Y:" },
            { "Z", "/C fsutil volume diskfree Z:" }
        };
        if (!allowedDrives.ContainsKey(drive))
        {
            drive = "C";
        }
        Process process = new Process();
        process.StartInfo.FileName = @"cmd.exe";
        process.StartInfo.Arguments = allowedDrives[drive];
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        double freeSpacePercent = double.Parse(output.Trim().Split(' ')[6]);

        process.WaitForExit();

        if (freeSpacePercent > 10)
        {
            return HealthCheckResult.Healthy("The check indicates a healthy result.");
        }
        else
        {
            return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
        }


    }
}
