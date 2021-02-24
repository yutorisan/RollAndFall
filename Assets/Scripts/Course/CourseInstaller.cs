using RAF.Course;
using UnityEngine;
using Zenject;

public class CourseInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ICourseGenerator>()
                 .To<CourseGenerator>()
                 .FromComponentInHierarchy()
                 .AsCached();
        Container.Bind<ICourseSwitcher>()
                 .To<CourseSwitcher>()
                 .FromComponentInHierarchy()
                 .AsCached();
    }
}