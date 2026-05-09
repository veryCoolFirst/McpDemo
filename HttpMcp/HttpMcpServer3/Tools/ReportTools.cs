using System.ComponentModel;
using System.Data;
using System.Text.Json;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace HttpMcpServer3.Tools;

[McpServerToolType]
public class ReportTools
{
    private const string ApiCenterUrl = "http://localhost:5246/";

    [McpServerTool]
    [Description("查询实时/历史新增报表，该工具会返回一个序列化过的Json格式数据")]
    public async Task<CallToolResult> QueryRealHisIncrease(
        RequestContext<CallToolRequestParams> context,
        [Description("指标配置，多个以,符号拼接，允许输入的值：Default value : cost_money,actual_cost_money,install_counts,install_actual_cost,register_counts,register_ratio,register_actual_cost,install_purchase_counts,install_purchase_amount,install_purchase_net_amount,install_purchase_ratio,install_purchase_actual_cost,install_purchase_roi,purchase_roi,install_purchase_actual_roi,purchase_actual_roi,ARPU,ARPPU,RR1N,RR3N,RR7N,LTV0,LTV3,LTV7,purchase_counts,purchase_amount,purchase_net_amount,purchase_ratio,purchase_actual_cost")] string Indicators,
        [Description("维度配置，多个以,符号拼接，允许输入的值：channel_type,channel_agency,channel_director,ad_account_id,ad_account_name,ad_plan_id,ad_plan_name")] string CheckedItems,
        [Description("游戏代码，默认值：SVM030")] string ServiceId = "SVM030",
        [Description("游戏应用Key，多个以,符号拼接，默认值：80285fa4b487b13a")] string AppKey = "80285fa4b487b13a",
        [Description("开始日期，默认值：20250620，数据最大范围：20250601-20250630")] string BeginDate = "20250620",
        [Description("结束日期，默认值：202506222，数据最大范围：20250601-20250630")] string EndDate = "202506222")
    // [Description("媒体类型，多个以,符号拼接，默认值：ALL")] string ChannelType = "ALL",
    // [Description("投放账户ID，多个以,符号拼接，默认值：ALL")] string AdAccountId = "ALL",
    // [Description("广告计划ID，多个以,符号拼接，默认值：ALL")] string AdPlanId = "ALL",
    // [Description("代理商，多个以,符号拼接，默认值：ALL")] string ChannelAgency = "ALL",
    // [Description("负责人，多个以,符号拼接，默认值：ALL")] string ChannelDirector = "ALL"
    {
        ArgumentNullException.ThrowIfNull(context.Services);

        var logger = context.Services.GetRequiredService<ILogger<ReportTools>>();
        var httpClientFactory = context.Services.GetRequiredService<IHttpClientFactory>();
        using var httpClient = httpClientFactory.CreateClient();

        httpClient.BaseAddress = new Uri(ApiCenterUrl);

        var requestUri = $"Report/RealHisIncrease?ServiceId={ServiceId}&Indicators={Indicators}&CheckedItems={CheckedItems}&AppKey={AppKey}&BeginDate={BeginDate}&EndDate={EndDate}&ChannelType=ALL&AdAccountId=ALL&AdPlanId=ALL&ChannelAgency=ALL&ChannelDirector=ALL&DataType=1&EventType=2&DateType=1&InvokeUser=AdBuyingAnalysisTest&Sign=123";

        logger.LogInformation("请求地址：{}", requestUri);

        var response = await httpClient.GetAsync(requestUri);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("请求出错，HTTP状态码：{}", response.StatusCode);
            return new CallToolResult
            {
                Content = [new TextContentBlock { Text = $"请求出错，HTTP状态码：{response.StatusCode}" }]
            };
        }

        string respContent = await response.Content.ReadAsStringAsync();
        using JsonDocument jsonDoc = JsonDocument.Parse(respContent);
        if (jsonDoc.RootElement.TryGetProperty("ReturnCode", out var code) && code.GetString() == "9")
        {
            logger.LogInformation("查询实时/历史新增报表接口成功");

            jsonDoc.RootElement.TryGetProperty("ReturnContent", out var content);

            return new CallToolResult
            {
                Content = [new TextContentBlock { Text = $"查询实时/历史新增报表接口成功，查询结果：{content}" }]
            };
        }
        else
        {
            jsonDoc.RootElement.TryGetProperty("ReturnMsg", out var message);

            logger.LogWarning("查询实时/历史新增报表接口失败 [{message}]", message);

            return new CallToolResult
            {
                Content = [new TextContentBlock { Text = $"查询实时/历史新增报表接口失败 [{message}]" }]
            };
        }
    }
}