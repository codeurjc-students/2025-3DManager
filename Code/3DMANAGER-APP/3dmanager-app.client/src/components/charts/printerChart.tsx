import {
    BarChart as ReBarChart,
    Bar,
    XAxis,
    YAxis,
    Tooltip,
    ResponsiveContainer,
    CartesianGrid,
    Rectangle,
} from "recharts";

const CustomBar = (props: any) => {
    const { fill, x, y, width, height } = props;
    return <Rectangle x={x} y={y} width={width} height={height} fill={fill} />;
};

export interface PrintChartData {
    name: string;
    value: number;
}

interface Props {
    data: PrintChartData[];
}

export const PrintChart = ({ data }: Props) => {
    const colors = ["#ffd54a", "#ba2020"];

    return (
        <ResponsiveContainer width="80%" height="80%">
            <ReBarChart className="" data={data} layout="vertical">
                <CartesianGrid strokeDasharray="10 10" />
                <YAxis type="category" dataKey="name" width={100} />
                <XAxis type="number" />
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


