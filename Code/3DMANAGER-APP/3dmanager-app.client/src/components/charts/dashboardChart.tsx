import {
    PieChart as RePieChart,
    Pie,
    Tooltip,
    Cell,
    ResponsiveContainer,
} from "recharts";

export interface DashboardChartData {
    name: string;
    value: number;
}

interface Props {
    data: DashboardChartData[];
    colors?: string[];
    height?: number;
}

export const DashboardChart = ({
    data,
    colors = ["#8884d8", "#82ca9d", "#ffc658"],
    height = 300,
}: Props) => {
    return (
        <ResponsiveContainer width="100%" height={height}>
            <RePieChart>
                <Tooltip />
                <Pie
                    data={data}
                    dataKey="value"
                    nameKey="name"
                    outerRadius={100}
                    fill="#8884d8"
                    label
                >
                    {data.map((_, index) => (
                        <Cell key={index} fill={colors[index % colors.length]} />
                    ))}
                </Pie>
            </RePieChart>
        </ResponsiveContainer>
    );
};
