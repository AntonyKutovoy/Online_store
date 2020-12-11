using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Shop
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //��������� ��������� ����������� � ������������� (MVC)
            services.AddControllersWithViews()
                //���������� ������������� � asp.net core 3.0
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //����� ����� ��������� ����� ������� ����������� middleware

            //� �������� ���������� ����� ������ ��������� ���������� �� �������
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //����������� ��������� ��������� ������ � ����������(css, js � �.�.)
            app.UseStaticFiles();

            //����������� ������ ��� ��������� (����������)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute("default", "{controller-Home}/{action-Index}/{id?}");
            //});
        }
    }
}
