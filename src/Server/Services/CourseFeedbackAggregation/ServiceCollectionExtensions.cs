using Microsoft.Extensions.DependencyInjection;

namespace KnowledgeShare.Server.Services.CourseFeedbackAggregation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCourseFeedbackAggregation(
            this IServiceCollection services)
        {
            services.AddSingleton<ICourseFeedbackAggregationQueue, CourseFeedbackAggregationQueue>();
            services.AddSingleton<CourseFeedbackAggregationService>();
            return services;
        }
    }
}
