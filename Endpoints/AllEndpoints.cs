using System;

namespace KpitApi.Endpoints;

public static class AllEndpoints
{
    public static void AddAllEndpoints(this WebApplication app)
    {
        app.AddDepartmentEndpoints();
        app.AddStudentEndpoints();
        app.AddAllCourseEndpoints();
    }
}
