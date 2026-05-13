import {
    BarChart as ReBarChart,
    Bar,
    XAxis,
    YAxis,
    Tooltip,
    ResponsiveContainer,
    Rectangle
} from "recharts";
import type { RectangleProps } from "recharts";

const CustomBar = (props: RectangleProps) => {
    const { fill, x = 0, y = 0, width = 0, height = 0 } = props;
    return <Rectangle x={x} y={y} width={width} height={height} fill={fill} />;
};

interface Props {
    data: { name: string; value: number }[];
    height?: number;
}

export const DashboardBarChart = ({ data, height = 300 }: Props) => {
    const colors = [
        "#ffd54a",
        "#fcdd74",
        "#fce9a3",
        "#2c2c2c",
        "#4a4a4a",
        "#6e6e6e",
        "#000000",
    ];


    return (
        <ResponsiveContainer width="100%" height={height}>
            <ReBarChart data={data} layout="vertical">
                <XAxis
                    type="number"
                    orientation="top"
                    label={{ value: "Horas", position: "insideTop", offset: -5 }}
                />
                <YAxis type="category" dataKey="name" width={150} />
                <Tooltip />

                <Bar
                    dataKey="value"
                    shape={(props) => {
                        const index = props.index;
                        const color = colors[index % colors.length];
                        return <CustomBar {...props} fill={color} />;
                    }}
                />
            </ReBarChart>
        </ResponsiveContainer>
    );
};
