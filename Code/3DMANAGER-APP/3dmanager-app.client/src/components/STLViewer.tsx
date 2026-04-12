import { useEffect, useRef } from "react";
import * as THREE from "three";
import { STLLoader } from "three/examples/jsm/loaders/STLLoader";
import { OrbitControls } from "three/examples/jsm/controls/OrbitControls";

interface STLViewerProps {
    fileUrl: string;
}

export const STLViewer = ({ fileUrl }: STLViewerProps) => {
    const mountRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        if (!mountRef.current || !fileUrl) return;

        const width = mountRef.current.clientWidth;
        const height = mountRef.current.clientHeight;

        const scene = new THREE.Scene();
        scene.background = new THREE.Color(0xffffff);

        const camera = new THREE.PerspectiveCamera(60, width / height, 1, 2000);
        camera.position.set(150, 150, 150);

        const renderer = new THREE.WebGLRenderer({ antialias: true });
        renderer.setSize(width, height);
        renderer.shadowMap.enabled = true;
        renderer.shadowMap.type = THREE.PCFShadowMap;
        mountRef.current.appendChild(renderer.domElement);

        const controls = new OrbitControls(camera, renderer.domElement);
        controls.enableDamping = true;

        const ambient = new THREE.AmbientLight(0xffffff, 0.6);
        scene.add(ambient);

        const hemi = new THREE.HemisphereLight(0xffffff, 0xdddddd, 0.6);
        scene.add(hemi);

        const dirLight = new THREE.DirectionalLight(0xffffff, 0.8);
        dirLight.position.set(100, 200, 150);
        dirLight.castShadow = true;
        dirLight.shadow.mapSize.width = 1024;
        dirLight.shadow.mapSize.height = 1024;
        scene.add(dirLight);

        const loader = new STLLoader();

        fetch(fileUrl)
            .then(async (res) => {
                if (!res.ok) throw new Error("No se pudo descargar el STL");
                return res.arrayBuffer();
            })
            .then((buffer) => {
                const geometry = loader.parse(buffer);

                const material = new THREE.MeshStandardMaterial({
                    color: 0xffd54a, 
                    metalness: 0.1,
                    roughness: 0.4,
                });

                const mesh = new THREE.Mesh(geometry, material);
                mesh.castShadow = true;
                mesh.receiveShadow = true;

                geometry.computeBoundingBox();
                const box = geometry.boundingBox!;
                const size = new THREE.Vector3();
                box.getSize(size);

                const maxDim = Math.max(size.x, size.y, size.z);
                const scale = 100 / maxDim;
                mesh.scale.set(scale, scale, scale);

                const center = new THREE.Vector3();
                box.getCenter(center);
                mesh.position.sub(center.multiplyScalar(scale));

                mesh.rotation.x = -Math.PI / 2;

                scene.add(mesh);
            })
            .catch((err) => console.error("Error cargando STL:", err));

        const animate = () => {
            requestAnimationFrame(animate);
            controls.update();
            renderer.render(scene, camera);
        };
        animate();
        return () => {
            controls.dispose();
            scene.traverse((obj) => {
                if (obj instanceof THREE.Mesh) {
                    obj.geometry?.dispose();
                    if (Array.isArray(obj.material)) {
                        obj.material.forEach((m) => m.dispose());
                    } else {
                        obj.material?.dispose();
                    }
                }
            });
            renderer.dispose();
            renderer.forceContextLoss();
            renderer.domElement = null as any;
            mountRef.current?.replaceChildren();
        };
    }, [fileUrl]);

    return (
        <div
            ref={mountRef}
            style={{ width: "100%", height: "100%", border: "1px solid #ccc" }}
        />
    );
};
