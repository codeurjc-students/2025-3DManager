import { render, screen } from '@testing-library/react';
import App from '../../App';

test('renders app title and disconnected status', () => {
    render(<App />);
    expect(screen.getByText('No conectado')).toBeInTheDocument();
});